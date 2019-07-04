using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterText : MonoBehaviour
{
    public MeshRenderer Border;
    public Material RedMaterial;
    public Material BlueMaterial;
    public Material NeutralMaterial;

	public Text redPointsText;
	public Text bluePointsText;
    public Text timeText;
    public Text stateText;
    public Color BlueColor;
    public Color RedColor;
    public Color InfoColor;

    const float updateInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateScoreboard());
    }

    IEnumerator UpdateScoreboard()
    {
        yield return new WaitForSeconds(updateInterval);

        redPointsText.text = GameManager.instance.GetRedPoints().ToString();
        bluePointsText.text = GameManager.instance.GetBluePoints().ToString();

        float playedTime = GameManager.instance.GetCurrentPlayedTime();
        int minutes = (int)(playedTime / 60);
        int seconds = (int)(playedTime % 60);
        timeText.text = (minutes < 10 ? "0" + minutes : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds : seconds.ToString());

        bool bIAmBlue = GameManager.instance.IsClient;
        bool bMyTurn = GameManager.instance.MyTurn;

        switch (GameManager.instance.GameState)
        {
            case GameManager.EGameState.WaitingForConnection:
                stateText.text = "Waiting for opponent...";
                stateText.color = InfoColor;
                Border.sharedMaterial = NeutralMaterial;
                break;
            case GameManager.EGameState.Pause:
                stateText.text = "Ready for next round...";
                stateText.color = InfoColor;
                Border.sharedMaterial = NeutralMaterial;
                break;
            case GameManager.EGameState.Running:
                stateText.text = bMyTurn ? "It's your turn!" : "It's the enemys turn!";
                stateText.color = bIAmBlue == bMyTurn ? BlueColor : RedColor;
                Border.sharedMaterial = bIAmBlue == bMyTurn ? BlueMaterial : RedMaterial;
                break;
        }

        yield return StartCoroutine(UpdateScoreboard());
    }
}
