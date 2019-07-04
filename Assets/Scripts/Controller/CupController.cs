using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking;
using UnityEngine;
using Valve.VR.InteractionSystem;


// NOTE: Interpolating the cups seems like a BAD idea...


[RequireComponent(typeof(Rigidbody))]
public class CupController : SyncedCupBehavior
{
    public CupBundleController father;
    private Rigidbody body;
    private bool sync = true;
    private bool bInit = false;

    private Interactable _interactable;
    private Throwable _throwable;
    private Hand.AttachmentFlags _defaultAttachmentFlagsForThroable;


    void Start()
    {
        Init();
        _defaultAttachmentFlagsForThroable = _throwable.attachmentFlags;
    }

    public void Init()
    {
        if (bInit) return;

        // no check required because of RequireComponent
        body = GetComponent<Rigidbody>();
        _interactable = GetComponent<Interactable>();
        _throwable = GetComponent<Throwable>();

        bInit = true;
    }

    private void FixedUpdate()
    {
        if (!sync || networkObject == null) return;

        if (networkObject.IsOwner)
        {
            body.useGravity = true;
            body.isKinematic = false;
            networkObject.position = transform.position;
            networkObject.rotation = transform.rotation;
            //Debug.Log("Sending Cups");
        }
        else
        {
            body.useGravity = false;
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            //body.MovePosition(networkObject.position);
            //body.MoveRotation(networkObject.rotation);
            transform.position = networkObject.position;
            transform.rotation = networkObject.rotation;
            //Debug.Log("Receiving Cups");
        }
    }

    //private void SetInterpolation(bool enabled)
    //{
    //    if (enabled == networkObject.positionInterpolation.Enabled) return;

    //    networkObject.positionInterpolation.current = transform.position;
    //    networkObject.positionInterpolation.target = transform.position;
    //    networkObject.positionInterpolation.Timestep = 0;

    //    networkObject.rotationInterpolation.current = transform.rotation;
    //    networkObject.rotationInterpolation.target = transform.rotation;
    //    networkObject.rotationInterpolation.Timestep = 0;

    //    networkObject.positionInterpolation.Enabled = enabled;
    //}

    public void SetSync(bool sync)
    {
        //SetInterpolation(sync);
        this.sync = sync;
    }

    public void SetOwnership(bool IOwnThis)
    {
        if (!networkObject.IsServer) return;

        networkObject.AssignOwnership(NetworkManager.Instance.Networker.Players[IOwnThis ? 0 : 1]);
    }

    public void SetActive(bool Active)
    {
        networkObject.SendRpc(RPC_SET_CUP_ACTIVE, Receivers.Others, Active);
        gameObject.SetActive(Active);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        //body.MovePosition(homePosition);
        //body.MoveRotation(homeRotation);
        transform.position = networkObject.homePosition;
        transform.rotation = networkObject.homeRotation;
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        gameObject.SetActive(true);
    }

    // RPC, do not call directly!
    public override void SetCupActive(RpcArgs args)
    {
        gameObject.SetActive(args.GetNext<bool>());
        SwitchThrowable(args.GetNext<bool>());
    }

    private void SwitchThrowable(bool getNext)
    {
        _interactable.enabled = getNext;
        _throwable.enabled = getNext;
        if (getNext)
            _throwable.attachmentFlags = _defaultAttachmentFlagsForThroable;
        else
            _throwable.attachmentFlags = 0;
    }
}