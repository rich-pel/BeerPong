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
        playerPointsText.text = "Your points: + playerPoints.ToString()";
		enemyPointsText.text = "Enemys points: + enemyPoints.ToString()";    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
