using System;
using System.Collections;
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

    private const float ballTimeoutSeconds = 10.0f;
    private bool ballInteracted = false;

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

            ballInteracted = false;
            StartCoroutine(StartBallTimeout());
        }
    }

    public void ResetGrabState()
    {
        ballInteracted = false;
    }

    IEnumerator StartBallTimeout()
    {
        yield return new WaitForSeconds(ballTimeoutSeconds);
        if (!ballInteracted)
        {
            GameManager.instance.BallTimedOut();
        }
    }

    public void BallIsGrabbed()
    {
        GameManager.instance.BallWasGrabbed();
    }

    public bool AmIOwnerOfBall()
    {
        return throwableBall.networkObject.IsOwner;
    }

    public void BallInteracted(string gameObjectTag)
    {
        ballInteracted = true;

        //Maybe here use the tag names also for the audio files -> just for performance
        //AudioManager.instance.Play(gameObjectTag)

        //Maybe just if the collision enter the ballFallBeside
        if (gameObjectTag.Equals("Ground"))
        {
            AudioManager.instance.Play("BallHitGround");
        }
        else if (gameObjectTag.Equals("Wall"))
        {
            AudioManager.instance.Play("BallHitWall");
        }
        else if (gameObjectTag.Equals("Table"))
        {
            audioCountUp++;
            AudioManager.instance.Play("BallHitTable" + audioCountUp);
            if (audioCountUp >= ballDippingMax)
                audioCountUp = 0;
        }
        else if (gameObjectTag.Equals("Counter"))
        {
            AudioManager.instance.Play("BallHitCounter");
        }

        GameManager.instance.BallFellBeside();
    }

    public void BallFellInCup(CupController cup)
    {
        ballInteracted = true;
        GameManager.instance.BallFellInCup(cup);
    }

    public bool BallFallsOutOfTheRoom(Vector3 ballPosition)
    {
        return Vector3.Distance(ballPosition, GameManager.instance.GetGroundPosition()) > _maxDistanceFromRoom;
    }
}