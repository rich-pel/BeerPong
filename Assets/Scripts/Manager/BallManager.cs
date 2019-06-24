using BeardedManStudios.Forge.Networking.Unity;
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
    public GameObject playersBallHolderArea;
    public GameObject enemysBallHolderArea;
    public int moveableTimeForBall = 10;

    private float timeOutForBall;
    private bool ballTimeIsTracked = false;

    private int audioCountUp = 0;
    [SerializeField] private int ballDippingMax = 3;
    private Rigidbody ballBody;


    // Start is called before the first frame update
    void Start()
    {
        // deactivate whole script if we're not server
        gameObject.SetActive(GameManager.instance.IsServer());
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
    }

    // Should only be called by the Server!
    public void SetPositionToBallHolder(bool myTurn)
    {
        if (myTurn)
        {
            throwableBall.transform.position = playersBallHolderArea.transform.position;
            ballBody.velocity = Vector3.zero;
            ballBody.angularVelocity = Vector3.zero;
            Debug.Log("I have the Ball");
        }
        else
        {
            throwableBall.transform.position = enemysBallHolderArea.transform.position;
            ballBody.velocity = Vector3.zero;
            ballBody.angularVelocity = Vector3.zero;
            Debug.Log("The enemy has the Ball");
        }

        if (throwableBall.networkObject != null && GameManager.instance.EnemyIsConnected())
        {
            throwableBall.SetOwnership(myTurn);
        }
    }


    public void BallIsGrabbed()
    {
        Debug.Log("Ball is Grabbed");
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
}