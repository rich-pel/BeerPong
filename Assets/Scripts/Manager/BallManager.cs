﻿using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton

    public static BallManager instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private BallController throwableBall;
    [SerializeField] private GameObject playersBallHolderArea;
    [SerializeField] private GameObject enemysBallHolderArea;
    [SerializeField] private int moveableTimeForBall = 10;

    private float timeOutForBall;
    private bool ballTimeIsTracked = false;

    private int audioCountUp = 0;
    [SerializeField] private int ballDippingMax = 3;
    private Rigidbody ballBody;
    [SerializeField] private float _maxDistanceFromRoom;


    // Start is called before the first frame update
    void Start()
    {
        // deactivate whole script if we're not server
        gameObject.SetActive(GameManager.instance.IsServer);

        if (!throwableBall)
        {
            Debug.LogError("throwableBall is not set!");
            return;
        }

        ballBody = throwableBall.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BallIsInAction())
        {
            ballTimeIsTracked = !ballTimeIsTracked;
            GameManager.instance.BallFellBeside();
        }

        if (GetCurrentTimeLeft() <=0)
        {
            GameManager.instance.BallFellBeside();
        }
    }

    public void DetachFromHand()
    {
        throwableBall.DetachFromHand();
    }

    public void SetBallState(BallController.EBallState NewState)
    {
        throwableBall.SetState(NewState);
    }

    // Should only be called by the Server!
    public void SetPositionToBallHolder(bool myTurn)
    {
        // if we're Client, the enemys BallHolderArea is our Area
        if (GameManager.instance.IsClient) myTurn = !myTurn;

        if (myTurn)
        {
            throwableBall.SetState(BallController.EBallState.WaitForGrab);
            throwableBall.gameObject.SetActive(false);
            throwableBall.transform.position = playersBallHolderArea.transform.position;
            throwableBall.gameObject.SetActive(true);
            Debug.Log(GameManager.instance.IsClient ? "The ENEMY has the Ball" : "I have the Ball");
        }
        else
        {
            throwableBall.SetState(BallController.EBallState.WaitForGrab);
            throwableBall.gameObject.SetActive(false);
            throwableBall.transform.position = enemysBallHolderArea.transform.position;
            throwableBall.gameObject.SetActive(true);
            Debug.Log(GameManager.instance.IsClient ? "I have the Ball" : "The ENEMY has the Ball");
        }

        ballBody.velocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
    }

    public void SetBallOwnership(bool myBall)
    {
        if (GameManager.instance.IsServer && GameManager.instance.GameState == GameManager.EGameState.Running)
        {
            throwableBall.SetOwnership(myBall);
        }
    }

    public bool AmIOwnerOfBall()
    {
        return throwableBall.networkObject.IsOwner;
    }

    public void BallIsGrabbed()
    {
        ballTimeIsTracked = true;
        timeOutForBall = Time.time + 10;
        GameManager.instance.BallWasGrabbed();
    }

    public float GetCurrentTimeLeft()
    {
        if (ballTimeIsTracked)
            return timeOutForBall - Time.time;
        else
            return moveableTimeForBall;
    }

    public void BallInteracted(string gameObjectTag)
    {
        //Maybe here use the tag names also for the audio files -> just for performance
        //AudioManager.instance.Play(gameObjectTag)
        
        //Maybe just if the collision enter the ballFallBeside
        if (gameObjectTag.Equals("Ground"))
        {
            GameManager.instance.BallFellBeside();
            AudioManager.instance.Play("BallHitGround");
        }
        else if (gameObjectTag.Equals("Wall"))
        {
            GameManager.instance.BallFellBeside();

            AudioManager.instance.Play("BallHitWall");
        }
        else if (gameObjectTag.Equals("Table"))
        {
            GameManager.instance.BallFellBeside();

            audioCountUp++;
            AudioManager.instance.Play("BallHitTable" + audioCountUp);
            if (audioCountUp >= ballDippingMax)
                audioCountUp = 0;
        }
        else if (gameObjectTag.Equals("Counter"))
        {
            GameManager.instance.BallFellBeside();
            AudioManager.instance.Play("BallHitCounter");
        }
    }

    public bool BallIsInAction()
    {
        return ballTimeIsTracked && timeOutForBall < Time.time;
    }

    public bool BallFallsOutOfTheRoom(Vector3 ballPosition)
    {
        return Vector3.Distance(ballPosition, GameManager.instance.GetGroundPosition()) > _maxDistanceFromRoom;
    }
}