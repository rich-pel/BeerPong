using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterText : MonoBehaviour
{

	public Text playerPointsText;
	public Text enemyPointsText;
	public int playerPoints;
	public int enemyPoints;


    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        playerPointsText.text = "Your points: "+ GameManager.instance.GetPlayerPoints();
		enemyPointsText.text = "Enemys points: "+ GameManager.instance.GetEnemyPoints();
    }
}
