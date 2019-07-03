using Valve.VR;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public float SmoothTime = 1.0f;

    public Transform VRRoot;
    public Transform VRHead;
    public Transform Destination;

    Vector3 velocity;

    public SteamVR_Action_Boolean Input;
    public SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.Any;
    
    // Use this for initialization
    void OnEnable()
    {
        if (Input != null)
        {
            Input.AddOnChangeListener(OnTriggerPressedOrReleased, InputSource);
        }
    }

    private void OnDisable()
    {
        if (Input != null)
        {
            Input.RemoveOnChangeListener(OnTriggerPressedOrReleased, InputSource);
        }
    }

    private void OnTriggerPressedOrReleased(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            ResetPosition();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //void Update()
    //{
    //    SteamVR_Input.GetStateDown
    //    foreach (var action in SteamVR_Input.actionsIn)
    //    {
    //        Debug.Log("ACTION: " + action);
    //    }
    //}

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    Vector3 dist = Destination.position - VRHead.position;
    //    VRRoot.position = Vector3.SmoothDamp(VRRoot.position, dist, ref velocity, SmoothTime);
    //}

    public void ResetPosition()
    {
        VRRoot.position = Destination.position - (VRHead.position - VRRoot.position);
        //VRRoot.rotation = Destination.rotation * Quaternion.Inverse(VRHead.rotation * Quaternion.Inverse(VRRoot.rotation));
    }
}
