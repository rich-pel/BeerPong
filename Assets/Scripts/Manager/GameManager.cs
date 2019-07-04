using System;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    const int MaxPoints = 10; // we keep this (although calculateable through cups) to be more efficient BlurController
    const int MaxTries = 3;
    const float StartCountdown = 3.0f; // in seconds

    [SerializeField] GameObject RoomGround;
    [SerializeField] Transform PlayerRedStart;
    [SerializeField] Transform PlayerBlueStart;
    
    public EGameState GameState
    {
        get { return (EGameState)networkObject.gameState; }
        private set { networkObject.gameState = (byte)value; }
    }
    
    public bool MyTurn { get; private set; }

    bool bInit = false;
    int currentTry = 0;
    bool waitForHandshake = false;

    void Start()
    {
        if (!RoomGround) Debug.LogError("RoomGround is NULL!");
        if (!PlayerRedStart) Debug.LogError("PlayerRedStart is NULL!");
        if (!PlayerBlueStart) Debug.LogError("PlayerBlueStart is NULL!");
    }

    // Start is called before the first frame update
    void Init()
    {
        if (IsServer)
        {
            CupManager.instance.InitCups(); // this must happen before Reset!
            Reset(false);
            GameState = EGameState.WaitingForConnection;

            NetworkManager.Instance.Networker.playerDisconnected += ThreadPipe.Pipe(OnPlayerDisconnected);
            NetworkManager.Instance.Networker.playerRejected += ThreadPipe.Pipe(OnPlayerRejected);
            NetworkManager.Instance.Networker.playerAccepted += ThreadPipe.Pipe(OnPlayerAccepted);
        }
        else if (IsClient)
        {
            NetworkManager.Instance.Networker.disconnected += ThreadPipe.Pipe(OnDisconnected);

            // teleport us to proper position
            PlayerController.Instance.Destination = PlayerBlueStart;
            PlayerController.Instance.ResetPosition();
            PlayerController.Instance.ShowOpponent = true;
        }
    }

    // Server Event
    void OnPlayerDisconnected(NetworkingPlayer player, NetWorker sender)
    {
        Debug.Log("Player " + player.Ip + " disconnected!");

        Reset(false);
        GameState = EGameState.WaitingForConnection;
        PlayerController.Instance.ShowOpponent = false;
    }

    // Server Event
    void OnPlayerRejected(NetworkingPlayer player, NetWorker sender)
    {
        Debug.Log("Player " + player.Ip + " rejected!");
    }

    // Server Event
    void OnPlayerAccepted(NetworkingPlayer player, NetWorker sender)
    {
        Debug.Log("Player " + player.Ip + " accepted!");
        Reset(true);
        PlayerController.Instance.ApplyBlueOwnership();
        PlayerController.Instance.ShowOpponent = true;
    }

    // Client Event
    void OnDisconnected(NetWorker sender)
    {
        Debug.Log("Disconnected Event!");
        BackToMainMenu();
    }

    void BackToMainMenu()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.Disconnect();
            //Destroy(NetworkManager.Instance.gameObject); // null exception?
        }

        if (BMSLogger.Instance != null) Destroy(BMSLogger.Instance.gameObject);
        if (MainThreadManager.Instance != null) Destroy(MainThreadManager.Instance.gameObject);

        if (PlayerController.Instance != null)
        {
            Destroy(PlayerController.Instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        // Escape back to Main Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }

        // If unity's Update() runs, before the object is
        // instantiated in the network, then simply don't
        // continue, otherwise a bug/error will happen.
        // 
        // Unity's Update() running, before this object is instantiated
        // on the network is **very** rare, but better be safe 100%
        if (networkObject == null) return;

        // async init (Start) required because of networking
        if (!bInit)
        {
            Init();
            bInit = true;
        }

        // Execute piped events!
        ThreadPipe.ExecuteQueue();

        // ========== Client code ==========
        if (!networkObject.IsServer)
        {
            if (!CupManager.instance.AllCupsFound)
            {
                CupManager.instance.InitClientCups();
            }

            if (waitForHandshake)
            {
                bool amIOwnerOfBall = BallManager.instance.AmIOwnerOfBall();
                bool amIOwnerOfCups = CupManager.instance.AmIOwnerOfCups();

                if (MyTurn == amIOwnerOfBall && MyTurn == amIOwnerOfCups)
                {
                    ApplyTurn();
                    waitForHandshake = false;

                    // Handshake to Server
                    networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.Server, !MyTurn);

                    Debug.Log("Correct Ownership received! Handshake send to Server!");
                }
                else
                {
                    Debug.Log("Waiting for Ball Ownership ("+ amIOwnerOfBall + ") and Cups Ownership ("+ amIOwnerOfCups + ") to change to my Turn state ("+MyTurn+") ...");
                }
            }

            return;
        }

        // ========== Server code ==========

        if (GameState == EGameState.Running)
        {
            // The Server can instantly change the turn for Debug reasons
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space Hit: " + currentTry);
                SetTurn(!MyTurn);
            }

            networkObject.playedTime += Time.deltaTime;
            CheckGameOver();
        }
    }

    void Reset(bool bStartGame)
    {
        if (!IsServer) return;
        
        MyTurn = true;
        GameState = EGameState.Pause;
        currentTry = 0;
        networkObject.hostPoints = 0;
        networkObject.clientPoints = 0;
        networkObject.playedTime = 0;
        
        CupManager.instance.SetCupsOwnership(MyTurn);
        CupManager.instance.ResetCups(false);
        BallManager.instance.SetBallOwnership(MyTurn);
        BallManager.instance.SetPositionToBallHolder(MyTurn);

        Debug.Log("Game Resetted!");

        if (bStartGame)
        {
            StartCoroutine(Countdown());
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(StartCountdown);
        GameStart();
    }

    void GameStart()
    {
        Debug.Log("Start Game!");

        PlayerController.Instance.Destination = PlayerRedStart;
        PlayerController.Instance.ResetPosition();

        GameState = EGameState.Running;
        SetTurn(MyTurn);
    }

    // Returns true if Game is really over (or bad game state)
    bool CheckGameOver()
    {
        if (!IsServer) return true;
        
        if (GameState != EGameState.Running)
        {
            Debug.LogError("Called 'CheckGameOver' although NO Game is currently running!");
            return true;
        }

        if (networkObject.hostPoints >= MaxPoints)
        {
            Debug.Log("WE WON !!!!!!!!!!!");
            Reset(true);
            return true;
        }
        else if (networkObject.clientPoints >= MaxPoints)
        {
            Debug.Log("The Enemy won...................");
            Reset(true);
            return true;
        }

        return false;
    }


    public void BallFellInCup(CupController cup)
    {
        if (!IsServer || GameState != EGameState.Running || waitForHandshake) return;

        Debug.Log("In Ball Fell In Cup");

        cup.SetActive(false); // either way which cup got hit, it's going to be deactivated
        bool myCup = CupManager.instance.IsMyCup(cup);
        networkObject.hostPoints += !myCup ? 1 : 0;     // either way, if the enemy cup got hit, I score a point
        networkObject.clientPoints += myCup ? 1 : 0;    // either way, if my cup got hit, the enemy scores a point

        if (MyTurn && myCup) Debug.Log("I hit my own cup...");
        if (MyTurn && !myCup) Debug.Log("I scored a point!");
        if (!MyTurn && myCup) Debug.Log("The ENEMY scored a point!");
        if (!MyTurn && !myCup) Debug.Log("The ENEMY hit his own cup...");

        if (!CheckGameOver())
        {
            NextTry();
        }
    }

    public void BallFellBeside()
    {
        if (waitForHandshake)
        {
            Debug.LogWarning("Ignoring BallFellBeside because we're waiting for the Handshake!");
            return;
        }

        if (!IsServer || GameState != EGameState.Running) return;

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

    void NextTry()
    {
        if (!IsServer || GameState != EGameState.Running) return;
        
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
            BallManager.instance.DetachFromHand();
            BallManager.instance.SetBallState(BallController.EBallState.WaitForGrab);
            BallManager.instance.SetPositionToBallHolder(MyTurn);
            
            if (!MyTurn)
            {
                networkObject.SendRpc(RPC_CLIENT_NEXT_TRY, Receivers.Others);
            }

            Debug.Log("Try No. "+currentTry+"/"+MaxTries);
        }
    }

    // RPC, do not call directly!
    public override void ClientNextTry(RpcArgs args)
    {
        if (!IsClient)
        {
            Debug.LogError("ClientNextTry called on non-client. This should not happen!");
            return;
        }

        BallManager.instance.DetachFromHand();
        BallManager.instance.SetBallState(BallController.EBallState.WaitForGrab);
        BallManager.instance.SetPositionToBallHolder(MyTurn);
    }

    void SetTurn(bool IamNext)
    {
        if (!IsServer) return;
        
        currentTry = 0;
        MyTurn = IamNext;
        Debug.Log("It's " + (MyTurn ? "my" : "the ENEMYS") + " Turn!");

        waitForHandshake = true;
        BallManager.instance.SetBallOwnership(MyTurn);
        BallManager.instance.SetBallState(BallController.EBallState.Pause);
        BallManager.instance.SetPositionToBallHolder(MyTurn);
        CupManager.instance.SetCupsOwnership(MyTurn);
        CupManager.instance.SyncCups(false);

        networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.Others, !MyTurn);
    }

    void ApplyTurn()
    {
        CupManager.instance.ResetCups(true);
        BallManager.instance.SetPositionToBallHolder(MyTurn);
        BallManager.instance.SetBallState(BallController.EBallState.WaitForGrab);
        CupManager.instance.SyncCups(true);
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
                ApplyTurn();
                Debug.Log("Turn Handshake successfull! myTurn: " + MyTurn);
            }
            else
            {
                Debug.LogError("Handshake mismatch! My turn: " + MyTurn + " while the Client thinks: " + incomingTurn);
                return;
            }
        }
        else
        {
            waitForHandshake = true;
            MyTurn = incomingTurn;
            BallManager.instance.SetBallState(BallController.EBallState.Pause);
            CupManager.instance.SyncCups(false);

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