using BeardedManStudios.Forge.Networking.Generated;
using Valve.VR.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
public class BallController : SyncedBallBehavior
{
    Throwable throwable;

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
        UpdateNetworkPosition();
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
}