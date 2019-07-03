using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class SyncedPlayerState : SyncedPlayerBehavior
{
    public bool ShowOpponent = false;
    public Transform VRHead;
    public Transform VRLeftHand;
    public Transform VRRightHand;
    public GameObject HeadPrefab;
    public GameObject HandPrefab;

    GameObject head = null;
    GameObject leftHand = null;
    GameObject rightHand = null;

    void Update()
    {
        if (networkObject == null) return;

        if (networkObject.IsOwner)
        {
            networkObject.headPosition = VRHead.transform.position;
            networkObject.headRotation = VRHead.transform.rotation;
            networkObject.rightHandPosition = VRRightHand.transform.position;
            networkObject.rightHandRotation = VRRightHand.transform.rotation;
            networkObject.leftHandPosition = VRLeftHand.transform.position;
            networkObject.leftHandRotation = VRLeftHand.transform.rotation;
            return;
        }

        if (ShowOpponent && head == null)
        {
            head = Instantiate(HeadPrefab, networkObject.headPosition, networkObject.headRotation);
            rightHand = Instantiate(HandPrefab, networkObject.headPosition, networkObject.headRotation);
            leftHand = Instantiate(HeadPrefab, networkObject.headPosition, networkObject.headRotation);

            // mirror hand model for left hand
            Vector3 scale = leftHand.transform.localScale;
            scale.x = -1f;
            leftHand.transform.localScale = scale;
        }
        else if (!ShowOpponent && head != null)
        {
            Destroy(head);
            Destroy(rightHand);
            Destroy(leftHand);
        }
        else if (ShowOpponent && head != null)
        {
            head.transform.position = networkObject.headPosition;
            head.transform.rotation = networkObject.headRotation;
            rightHand.transform.position = networkObject.rightHandPosition;
            rightHand.transform.rotation = networkObject.rightHandRotation;
            leftHand.transform.position = networkObject.leftHandPosition;
            leftHand.transform.rotation = networkObject.leftHandRotation;
        }
    }
}
