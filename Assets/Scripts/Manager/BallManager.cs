using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallManager : MonoBehaviour
{
    #region Singleton

    public static BallManager instance;

    public GameObject throwableBall;

    public GameObject playersBallHolderArea;

    public GameObject enemysBallHolderArea;

    public int moveableTimeForBall = 10;

    private float timeOutForBall;
    private bool ballTimeIsTracked = false;
    
    
    private void Awake()
    {
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ballTimeIsTracked && timeOutForBall < Time.time)
        {
            ballTimeIsTracked = !ballTimeIsTracked;
            GameManager.instance.BallFallBeside();
        }
    }

    public void SetPositionToBallHolder(bool enemysTurn)
    {
        if (enemysTurn)
        {
            throwableBall.transform.position = enemysBallHolderArea.transform.position;
        }
        else
        {
            throwableBall.transform.position = playersBallHolderArea.transform.position;
        }
    }


    public void BallIsGrabbed()
    {
        Debug.Log("Ball is Grabbed");
        ballTimeIsTracked = true;
        timeOutForBall = Time.time + 10;
    }

    public float getCurrentTimeLeft()
    {
        if (ballTimeIsTracked)
            return timeOutForBall - Time.time;
        else
            return moveableTimeForBall;
    }
}