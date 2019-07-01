using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CupController : SyncedCupBehavior
{
    public CupBundleController father;
    private Vector3 homePosition;
    private Quaternion homeRotation;
    private Rigidbody body;
    private bool bInit = false;

    void Start()
    {
        homePosition = transform.position;
        homeRotation = transform.rotation;
        body = GetComponent<Rigidbody>(); // no check required because of RequireComponent
    }

    private void Init()
    {
        if (!networkObject.IsServer)
        {
            body.useGravity = false;
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (networkObject == null) return;

        if (!bInit)
        {
            Init();
            bInit = false;
        }
    }

    private void FixedUpdate()
    {
        if (networkObject == null) return;

        if (networkObject.IsServer)
        {
            networkObject.position = body.position;
            networkObject.rotation = body.rotation;
        }
        else
        {
            body.MovePosition(networkObject.position);
            body.MoveRotation(networkObject.rotation);
        }
    }

    public void SetActive(bool Active)
    {
        networkObject.SendRpc(RPC_SET_CUP_ACTIVE, Receivers.Others, Active);
        gameObject.SetActive(Active);
    }

    public void Reset()
    {
        body.MovePosition(homePosition);
        body.MoveRotation(homeRotation);
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
    }

    // RPC, do not call directly!
    public override void SetCupActive(RpcArgs args)
    {
        gameObject.SetActive(args.GetNext<bool>());
    }
}