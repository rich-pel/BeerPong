using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SyncedBall))]
public class SyncedBallEditor : Editor
{
    int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SyncedBall theBall = (SyncedBall)target;
        if (theBall != null)
        {
            if (GUILayout.Button("Reset Rigidbody Velocity"))
            {
                var body = theBall.GetComponent<Rigidbody>();
                if (body)
                {
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero; // Euler Angles
                }
                else
                {
                    Debug.LogWarning("Ball does not have a Rigidbody!");
                }
            }

            if (theBall.networkObject != null && theBall.networkObject.IsServer)
            {
                NetWorker server = NetworkManager.Instance.Networker;

                NetworkingPlayer[] players = new NetworkingPlayer[server.Players.Count];
                string[] entries = new string[server.Players.Count];

                int i = 0;
                server.IteratePlayers((NetworkingPlayer player) =>
                {
                    players[i] = player;
                    entries[i] = i == 0 ? "Server" : "Client " + player.Ip;
                    ++i;
                });

                int newIndex = EditorGUILayout.Popup("", selectedIndex, entries);
                if (newIndex != selectedIndex)
                {
                    theBall.networkObject.AssignOwnership(players[newIndex]);
                    selectedIndex = newIndex;
                }
            }
        }
    }
}