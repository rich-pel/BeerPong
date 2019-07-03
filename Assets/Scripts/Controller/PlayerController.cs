using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
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
    public SyncedPlayerState RedPlayer;
    public SyncedPlayerState BluePlayer;

    public SteamVR_Action_Boolean Input;
    public SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.Any;


    public bool ShowOpponent
    {
        get
        {
            return GameManager.instance.IsServer && !GameManager.instance.IsClient && BluePlayer.ShowOpponent ||
                   GameManager.instance.IsClient && !GameManager.instance.IsServer && RedPlayer.ShowOpponent;
        }
        set
        {
            if (GameManager.instance.IsServer)
                BluePlayer.ShowOpponent = value;
            else if (GameManager.instance.IsClient)
                RedPlayer.ShowOpponent = value;
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        if (Input != null)
        {
            Input.AddOnChangeListener(OnTriggerPressedOrReleased, InputSource);
        }
    }

    void OnDisable()
    {
        if (Input != null)
        {
            Input.RemoveOnChangeListener(OnTriggerPressedOrReleased, InputSource);
        }
    }

    void OnTriggerPressedOrReleased(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        VRRoot.position = Destination.position - (VRHead.position - VRRoot.position);
    }

    public void ApplyBlueOwnership()
    {
        BluePlayer.networkObject.AssignOwnership(NetworkManager.Instance.Networker.Players[1]);
    }
}
