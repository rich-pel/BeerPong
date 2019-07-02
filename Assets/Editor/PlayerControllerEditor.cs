using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController player = (PlayerController)target;
        if (player != null)
        {
            if (GUILayout.Button("Reset VR position"))
            {
                player.ResetPosition();
            }
        }
    }
}