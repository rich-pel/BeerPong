using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Valve.VR.InteractionSystem;
using UnityEngine;
using BeardedManStudios.Forge.Networking;

[RequireComponent(typeof(Throwable))]
public class BallController : SyncedBallBehavior
{
    Throwable throwable;
    bool sync = true;
    bool nextOwnershipState = false;
    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        // this should never fail due to RequireComponent Attribute
        throwable = GetComponent<Throwable>();
    }

    // Update is called once per frame
    void Update()
    {
        // If unity's Update() runs, before the object is
        // instantiated in the network, then simply don't
        // continue, otherwise a bug/error will happen.
        // 
        // Unity's Update() running, before this object is instantiated
        // on the network is **very** rare, but better be safe 100%
        if (networkObject == null) return;

        //Debug.Log("Doing Networking 'n stuff");
        if (sync)
        {
            UpdateNetworkPosition();
        }
        else
        {
            transform.position = lastPosition;

            sync = networkObject.IsOwner == nextOwnershipState;
            if (sync)
            {
                ApplyChangedOwnership();
            }
        }
    }

    void ApplyChangedOwnership()
    {
        throwable.attachmentFlags = networkObject.IsOwner ? Hand.AttachmentFlags.VelocityMovement : 0;
        Debug.Log("Switched ownership of Ball to: " + (networkObject.IsOwner ? "ME" : "ENEMY"));
    }

    public void SetOwnership(bool IOwnThis)
    {
        if (!networkObject.IsServer) return;

        lastPosition = transform.position;
        sync = false;
        networkObject.SendRpc(RPC_NOTIFY_OWNERSHIP_CHANGE, Receivers.Others, !IOwnThis, transform.position);
        networkObject.AssignOwnership(NetworkManager.Instance.Networker.Players[IOwnThis ? 0 : 1]);

        ApplyChangedOwnership();
        sync = true;
    }

    public void UpdateNetworkPosition()
    {
        if (networkObject.IsOwner)
        {
            networkObject.position = transform.position;
            throwable.attachmentFlags = Hand.AttachmentFlags.VelocityMovement;
        }
        else
        {
            transform.position = networkObject.position;
            throwable.attachmentFlags = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        BallManager.instance.BallInteracted(other.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Cup"))
        {
            CupController cup = other.gameObject.GetComponent<CupController>();
            if (!cup)
            {
                Debug.LogError("Object tagged as Cup '"+other+"' does not have a CupController attached!");
                return;
            }
            GameManager.instance.BallFellInCup(cup);
            AudioManager.instance.Play("BallHitCup");
        }
    }

    public void WasTaken()
    {
        BallManager.instance.BallIsGrabbed();
        AudioManager.instance.Play("TakeBall");
    }

    public override void NotifyOwnershipChange(RpcArgs args)
    {
        throwable.attachmentFlags = 0;
        nextOwnershipState = args.GetNext<bool>();
        lastPosition = args.GetNext<Vector3>();
    }
}