using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Valve.VR.InteractionSystem;
using UnityEngine;
using BeardedManStudios.Forge.Networking;

[RequireComponent(typeof(Throwable), typeof(Rigidbody))]
public class BallController : SyncedBallBehavior
{
    bool _sync = true;
    public bool Sync
    {
        get { return _sync; }
        set
        {
            _sync = value;
            body.isKinematic = !value;
        }
    }
    Throwable throwable;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        // this should never fail due to RequireComponent Attribute
        throwable = GetComponent<Throwable>();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If unity's Update() runs, before the object is
        // instantiated in the network, then simply don't
        // continue, otherwise a bug/error will happen.
        // 
        // Unity's Update() running, before this object is instantiated
        // on the network is **very** rare, but better be safe 100%
        if (networkObject == null) return;

        if (Sync)
        {
            UpdateNetworkPosition();
        }
    }

    public void SetOwnership(bool IOwnThis)
    {
        if (!networkObject.IsServer) return;

        networkObject.AssignOwnership(NetworkManager.Instance.Networker.Players[IOwnThis ? 0 : 1]);
    }

    public void UpdateNetworkPosition()
    {
        if (networkObject.IsOwner)
        {
            body.useGravity = true;
            networkObject.position = body.position;
            throwable.attachmentFlags = Hand.AttachmentFlags.VelocityMovement;
        }
        else
        {
            body.useGravity = false;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.MovePosition(networkObject.position);
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
}