using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Valve.VR.InteractionSystem;
using UnityEngine;
using BeardedManStudios.Forge.Networking;

[RequireComponent(typeof(Throwable), typeof(Rigidbody), typeof(TrailRenderer))]
public class BallController : SyncedBallBehavior
{
    public enum EBallState
    {
        Pause, // no syncing, no interpolation
        WaitForGrab, // syncing, but no interpolation
        Game // syncing + interpolation
    }

    public EBallState State { get; private set; }
    Interactable interact;
    Throwable throwable;
    Rigidbody body;
    TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
        // this should never fail due to RequireComponent Attribute
        throwable = GetComponent<Throwable>();
        body = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

        // Throwable requires Interactable
        interact = GetComponent<Interactable>();

        SetState(EBallState.Pause);
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

        if (State != EBallState.Pause)
        {
            UpdateNetworkPosition();
        }

        if (BallFallsOutOfTheRoom())
        {
            GameManager.instance.BallFellBeside();
        }
    }

    private bool BallFallsOutOfTheRoom()
    {
        return BallManager.instance.BallFallsOutOfTheRoom(gameObject.transform.position);
    }

    public void DetachFromHand()
    {
        if (interact.attachedToHand != null)
        {
            interact.attachedToHand.DetachObject(gameObject);
        }
    }

    public void SetState(EBallState NewState)
    {
        if (State != NewState)
        {
            State = NewState;
            SetInterpolation(State == EBallState.Game);
            trail.enabled = State == EBallState.Game;
            Debug.Log("Set Ball State to: " + State);
        }
        else
        {
            Debug.LogWarning("Tried to Set Ball State to current State: " + State);
        }
    }

    // Called by SteamVR (see inspector)
    public void OnPickUp()
    {
        PlayerController.Instance.SetPlayerCollision(false);

        // Ignore grab if it's not our Turn or we already grabbed the ball once
        if (!GameManager.instance.MyTurn || State == EBallState.Game) return;

        BallGrabbed();

        // Inform our opponent that we grabbed the Ball
        networkObject.SendRpc(RPC_ON_ENEMY_PICKUP, Receivers.Others);

        Debug.Log("I Grabbed the Ball!");
        AudioManager.instance.Play("TakeBall");
    }

    public void OnDetach()
    {
        PlayerController.Instance.SetPlayerCollision(true);
    }

    private void BallGrabbed()
    {
        SetState(EBallState.Game);

        if (networkObject.IsServer)
        {
            BallManager.instance.BallIsGrabbed();
        }
    }

    private void SetInterpolation(bool enabled)
    {
        if (enabled == networkObject.positionInterpolation.Enabled)
        {
            //Debug.LogWarning("Tried to set Ball Interpolation state to current State: " + enabled);
            return;
        }

        networkObject.positionInterpolation.current = body.position;
        networkObject.positionInterpolation.target = body.position;
        networkObject.positionInterpolation.Timestep = 0;

        networkObject.positionInterpolation.Enabled = enabled;

        Debug.Log("Ball interpolation: " + enabled);
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
        if (State == EBallState.Game)
        {
            BallManager.instance.BallInteracted(other.gameObject.tag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (State == EBallState.Game && other.gameObject.tag.Equals("Cup"))
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

    public override void OnEnemyPickup(RpcArgs args)
    {
        BallGrabbed();
        Debug.Log("Enemy grabbed the Ball!");
    }
}