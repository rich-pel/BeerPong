using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterText : MonoBehaviour
{

	public Text playerPointstext;
	public Text enemyPointstext;
	public int playerPoints;
	public int enemyPoints;


    // Start is called before the first frame update
    void Start()
    {
        playerPointstext.text = "Your points: + playerPoints.ToString()";
		enemyPointstext.text = "Enemys points: + enemyPoints.ToString()";    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
