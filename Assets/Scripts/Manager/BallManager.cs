using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallManager : MonoBehaviour
{
    #region Singleton

    public static BallManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject throwableBall;
    public GameObject playersBallHolderArea;
    public GameObject enemysBallHolderArea;
    public int moveableTimeForBall = 10;

    private float timeOutForBall;
    private bool ballTimeIsTracked = false;
    
    

    // Start is called before the first frame update
    void Start()
    {
        // deactivate whole script if we're not server
        gameObject.SetActive(GameManager.instance.IsServer());
    }

    // Update is called once per frame
    void Update()
    {
        if (ballTimeIsTracked && timeOutForBall < Time.time)
        {
            ballTimeIsTracked = !ballTimeIsTracked;
            GameManager.instance.BallFellBeside();
        }
    }

    public void SetPositionToBallHolder(bool myTurn)
    {
        if (myTurn)
        {
            throwableBall.transform.position = playersBallHolderArea.transform.position;
        }
        else
        {
            throwableBall.transform.position = enemysBallHolderArea.transform.position;
        }
    }


    public void BallIsGrabbed()
    {
        Debug.Log("Ball is Grabbed");
        ballTimeIsTracked = true;
        timeOutForBall = Time.time + 10;
    }

    public float GetCurrentTimeLeft()
    {
        if (ballTimeIsTracked)
            return timeOutForBall - Time.time;
        else
            return moveableTimeForBall;
    }
}