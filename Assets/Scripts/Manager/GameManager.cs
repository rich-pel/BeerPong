using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private bool enemysTurn = false;

    private int playerPoints = 0;
    private int enemyPoints = 0;
    private float playedTime;

  

    // Start is called before the first frame update
    void Start()
    {
    	
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchBallPosition(enemysTurn);
            
        }

        playedTime = playedTime + Time.deltaTime;
    }

    public void BallFallInCup()
    {
        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fall In Cup");
        //Here has also to happend the update for the points and so on...
        if (enemysTurn)
            enemyPoints++;
        else
            playerPoints++;
        SwitchBallPosition(enemysTurn);
    }

    public void BallFallBeside()
    {
        //no counting for the points
        SwitchBallPosition(enemysTurn);
    }

    private void SwitchBallPosition(bool turn)
    {
        BallManager.instance.SetPositionToBallHolder(turn);
        enemysTurn = !turn;
    }

    public int GetPlayerPoints()
    {
        return playerPoints;
        
    }
    public int GetEnemyPoints()
    {
        return enemyPoints;
    	
	}

    public float getCurrentPlayedTime()
    {
        return playedTime;
    }

}