using BeardedManStudios.Forge.Networking.Generated;
using Valve.VR.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Throwable))]
public class BallController : SyncedBallBehavior
{
    public int audioCountUp = 0;
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
        //Maybe just if the collision enter the ballFallBeside
        if (other.gameObject.tag.Equals("Ground"))
        {
            GameManager.instance.BallFellBeside();
            AudioManager.instance.Play("BallHitGround");
        }

        else if (other.gameObject.tag.Equals("Wall"))
        {
            GameManager.instance.BallFellBeside();

            AudioManager.instance.Play("BallHitWall");
        }

        else if (other.gameObject.tag.Equals("Table"))
        {
            GameManager.instance.BallFellBeside();

            if (audioCountUp == 0)
            {
                AudioManager.instance.Play("BallHitTable1");
                audioCountUp++;
            }

            else if (audioCountUp == 1)
            {
                AudioManager.instance.Play("BallHitTable2");
                audioCountUp++;
            }

            else if (audioCountUp == 2)
            {
                AudioManager.instance.Play("BallHitTable3");
                audioCountUp = 0;
            }
        }

        else if (other.gameObject.tag.Equals("Counter"))
        {
            GameManager.instance.BallFellBeside();
            AudioManager.instance.Play("BallHitCounter");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Cup"))
        {
            other.gameObject.GetComponentInParent<CupController>().DeactivateTheCup();
            GameManager.instance.BallFellInCup();
            AudioManager.instance.Play("BallHitCup");
        }
    }

    public void WasTaken()
    {
        BallManager.instance.BallIsGrabbed();
        AudioManager.instance.Play("TakeBall");
    }
}