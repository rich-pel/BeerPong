using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class GameManager : GameManagerBehavior
{
    public enum EGameState
    {
        WaitingForConnection,
        Pause,
        Running
    }

    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public bool IsServer { get { return networkObject != null ? networkObject.IsServer : false; } }
    public bool IsClient { get { return networkObject != null ? !networkObject.IsServer : false; } }

    public const int MaxPoints = 10; // we keep this (although calculateable through cups) to be more efficient BlurController
    [SerializeField] private int MaxTries = 1;
    [SerializeField] private float StartCountdown = 3.0f;

    [SerializeField] private GameObject RoomGround;
    
    public EGameState gameState { get; private set; }
    
    public bool MyTurn { get; private set; }

    private bool bInit = false;
    private int currentTry = 0;
    
    private float countdownTimer = 0f;
    private bool waitForHandshake = false;


    // Start is called before the first frame update
    void Init()
    {
        if (IsServer)
        {
            Reset();
            gameState = EGameState.WaitingForConnection;

            NetworkManager.Instance.Networker.playerDisconnected += (NetworkingPlayer player, NetWorker sender) =>
            {
                Debug.Log("Player " + player.Ip + " disconnected!");
            };

            NetworkManager.Instance.Networker.playerRejected += (NetworkingPlayer player, NetWorker sender) =>
            {
                Debug.Log("Player " + player.Ip + " rejected!");
            };

            NetworkManager.Instance.Networker.playerAccepted += (NetworkingPlayer player, NetWorker sender) => 
            {
                Debug.Log("Player " + player.Ip + " accepted! Sending current turn...");
                networkObject.SendRpc(player, RPC_PLAYER_TURN, MyTurn);
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If unity's Update() runs, before the object is
        // instantiated in the network, then simply don't
        // continue, otherwise a bug/error will happen.
        // 
        // Unity's Update() running, before this object is instantiated
        // on the network is **very** rare, but better be safe 100%
        if (networkObject == null) return;

        // ========== Client code ==========
        if (!networkObject.IsServer)
        {
            if (waitForHandshake)
            {
                if (MyTurn == BallManager.instance.AmIOwnerOfBall())
                {
                    waitForHandshake = false;
                    BallManager.instance.SetPositionToBallHolder(MyTurn);
                    BallManager.instance.SetBallState(BallController.EBallState.WaitForGrab);

                    // Handshake to Server
                    networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.Server, !MyTurn);

                    Debug.Log("Correct Ownership received! Handshake send to Server!");
                }
                else
                {
                    Debug.Log("Waiting for Ownership (currently: "+BallManager.instance.AmIOwnerOfBall()+") to change to my Turn state (currently: "+MyTurn+") ...");
                }
            }

            return;
        }

        // ========== Server code ==========

        // async init (Start) required because of networking
        if (!bInit)
        {
            Init();
            bInit = true;
        }

        if (gameState == EGameState.WaitingForConnection && EnemyIsConnected())
        {
            Debug.Log("Enemy connected!");
            Reset();
        }
        else if (gameState == EGameState.Pause)
        {
            if (countdownTimer > 0f)
            {
                countdownTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Start Game!");
                Reset();
                gameState = EGameState.Running;
            }
        }
        else if (gameState == EGameState.Running)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space Hit: " + currentTry);
                //currentTry++; 
                SetTurn(!MyTurn);
            }

            networkObject.playedTime += Time.deltaTime;
            CheckGameOver();
        }

        // check if enemy is still connected...
        if (gameState != EGameState.WaitingForConnection && !EnemyIsConnected())
        {
            Debug.LogWarning("We lost the connection to our Enemy! Reset Game!");
            Reset();
            gameState = EGameState.WaitingForConnection;
        }
    }

    void Reset()
    {
        if (!IsServer) return;
        
        MyTurn = true;
        gameState = EGameState.Pause;
        countdownTimer = StartCountdown;
        currentTry = 0;
        networkObject.hostPoints = 0;
        networkObject.clientPoints = 0;
        networkObject.playedTime = 0;
        
        CupManager.instance.ResetAllCups();
        BallManager.instance.SetPositionToBallHolder(MyTurn);
        BallManager.instance.SetBallOwnership(MyTurn);
        
        Debug.Log("Game Resetted!");
    }

    void CheckGameOver()
    {
        if (!IsServer) return;
        
        if (gameState != EGameState.Running)
        {
            Debug.LogError("Called 'CheckGameOver' although NO Game is currently running!");
            return;
        }

        if (networkObject.hostPoints >= MaxPoints)
        {
            Debug.Log("WE WON !!!!!!!!!!!");
            Reset();
        }
        else if (networkObject.clientPoints >= MaxPoints)
        {
            Debug.Log("The Enemy won...................");
            Reset();
        }
    }

    public bool EnemyIsConnected()
    {
        return NetworkManager.Instance.Networker.Players.Count >= 2;
    }

    public void BallFellInCup(CupController cup)
    {
        if (!IsServer || gameState != EGameState.Running || waitForHandshake) return;
        
        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fell In Cup");
        
        //Here has also to happend the update for the points and so on...
        if (MyTurn)
        {
            if (CupManager.instance.IsMyCup(cup))
            {
                Debug.Log("You hit the wrong cup, idiot!");
            }
            else
            {
                cup.SetActive(false);
                networkObject.hostPoints++;
                Debug.Log("I scored a Point! My Points: " + networkObject.hostPoints);
                CheckGameOver();
            }
        }
        else
        {
            if (!CupManager.instance.IsMyCup(cup))
            {
                Debug.Log("Enemy hit the wrong cup, lol!");
            }
            else
            {
                cup.SetActive(false);
                networkObject.clientPoints++;
                Debug.Log("The Enemy scored a Point! Enemy Points: " + networkObject.clientPoints);
                CheckGameOver();
            }
        }

        NextTry();
    }

    public void BallFellBeside()
    {
        if (waitForHandshake)
        {
            Debug.LogWarning("Ignoring BallFellBeside because we're waiting for the Handshake!");
            return;
        }

        if (!IsServer || gameState != EGameState.Running) return;

        if (MyTurn)
        {
            Debug.Log("I can't aim...");
        }
        else
        {
            Debug.Log("The ENEMY can't aim!");
        }
        NextTry();
    }

    private void NextTry()
    {
        if (!IsServer) return;
        
        currentTry++;
        if (currentTry >= MaxTries)
        {
            // SetTurn will set currentTry back to 0 !
            // This has the advantage of being able to switch the turn 
            // mid game (if wished for some reason, e.g. space bar)
            SetTurn(!MyTurn);
        }
        else
        {
            Debug.Log("Try No. "+currentTry+"/"+MaxTries);
        }
    }
    
    private void SetTurn(bool IamNext)
    {
        if (!IsServer) return;
      
        currentTry = 0;
        MyTurn = IamNext;
        Debug.Log("It's " + (MyTurn ? "my" : "the ENEMYS") + " Turn!");

        CupManager.instance.StandActiveCupsBackToOringPos(MyTurn);
        BallManager.instance.SetBallState(BallController.EBallState.Pause);
        waitForHandshake = true;
        BallManager.instance.SetPositionToBallHolder(MyTurn);
        BallManager.instance.SetBallOwnership(MyTurn);

        networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.Others, !MyTurn);

        // TODO: Show the player some indication it's his turn!
    }

    public int GetRedPoints()
    {
        return networkObject != null ? networkObject.hostPoints : 0;
    }

    public int GetBluePoints()
    {
        return networkObject != null ? networkObject.clientPoints : 0;
    }

    public float GetCurrentPlayedTime()
    {
        return networkObject != null ? networkObject.playedTime : 0f;
    }

    // RPC, do not call directly!
    public override void PlayerTurn(RpcArgs args)
    {
        // NOTE: incoming Turn value should always be from our point of view!
        bool incomingTurn = args.GetNext<bool>();

        if (IsServer)
        {
            if (incomingTurn == MyTurn)
            {
                // Handshake successfull!
                waitForHandshake = false;
                BallManager.instance.SetBallState(BallController.EBallState.WaitForGrab);
                Debug.Log("Turn Handshake successfull! myTurn: " + MyTurn);
            }
            else
            {
                Debug.LogError("Handshake mismatch! My turn: " + MyTurn + " while the Client thinks: " + incomingTurn);
            }
        }
        else
        {
            BallManager.instance.SetBallState(BallController.EBallState.Pause);
            MyTurn = incomingTurn;
            waitForHandshake = true;

            Debug.Log("Receiving Turn - MyTurn: " + MyTurn);
        }

        // TODO: Show the player some indication it's his turn!
    }

    // RPC, do not call directly!
    public override void GameOver(RpcArgs args)
    {
        if (IsServer) return;

        // TODO: Show some end screen etc...
        throw new System.NotImplementedException();
    }

    public void BallWasGrabbed()
    {
        // TODO: this is dead right now...
        //currentTry++;
    }

    public Vector3 GetGroundPosition()
    {
        return RoomGround.gameObject.transform.position;
    }
}