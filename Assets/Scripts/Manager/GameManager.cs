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
    
    public const int MaxPoints = 10; // we keep this (although calculateable through cups) to be more efficient BlurController
    [SerializeField] private int MaxTries = 1;
    [SerializeField] private float StartCountdown = 3.0f;
    
    public EGameState gameState { get; private set; }
    
    private bool myTurn = false;
    private bool bInit = false;
    private int currentTry = 0;
    
    private float countdownTimer = 0f;



    // Start is called before the first frame update
    void Init()
    {
        if (IsServer())
        {
            Reset();
            gameState = EGameState.WaitingForConnection;
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
        if (!IsServer()) return;

        // async init (Start) required because of networking
        if (!bInit)
        {
            Init();
            bInit = true;
        }

        if (gameState == EGameState.WaitingForConnection)// && EnemyIsConnected())
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
                SetTurn(!myTurn);
            }

            networkObject.playedTime += Time.deltaTime;
            CheckGameOver();
        }

        // check if enemy is still connected...
        /*if (gameState != EGameState.WaitingForConnection && !EnemyIsConnected())
        {
            Debug.LogWarning("We lost the connection to our Enemy! Reset Game!");
            Reset();
            gameState = EGameState.WaitingForConnection;
        }*/
    }

    void Reset()
    {
        if (!IsServer()) return;
        
        myTurn = true;
        gameState = EGameState.Pause;
        countdownTimer = StartCountdown;
        currentTry = 0;
        networkObject.hostPoints = 0;
        networkObject.clientPoints = 0;
        networkObject.playedTime = 0;
        
        CupManager.instance.ResetAllCups();
        BallManager.instance.SetPositionToBallHolder(myTurn);
        
        Debug.Log("Game Resetted!");
    }

    void CheckGameOver()
    {
        if (!IsServer()) return;
        
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

    public bool IsServer()
    {
        return networkObject != null ? networkObject.IsServer : false;
    }

    public bool EnemyIsConnected()
    {
        return NetworkManager.Instance.Networker.Players.Count >= 2;
    }

    public void BallFellInCup(CupController cup)
    {
        if (!IsServer() || gameState != EGameState.Running) return;
        
        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fell In Cup");
        
        //Here has also to happend the update for the points and so on...
        if (myTurn)
        {
            if (CupManager.instance.IsMyCup(cup))
            {
                Debug.Log("You hit the wrong cup, idiot!");
            }
            else
            {
                cup.Deactivate();
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
                cup.Deactivate();
                networkObject.clientPoints++;
                Debug.Log("The Enemy scored a Point! Enemy Points: " + networkObject.clientPoints);
                CheckGameOver();
            }
        }

        NextTry();
    }

    public void BallFellBeside()
    {
        if (!IsServer() || gameState != EGameState.Running) return;

        Debug.Log("Someone can't aim, lol");
        NextTry();
    }

    private void NextTry()
    {
        if (!IsServer()) return;
        
        currentTry++;
        if (currentTry >= MaxTries)
        {
            SetTurn(!myTurn);
        }
        else
        {
            Debug.Log("Try No. "+currentTry+"/"+MaxTries);
        }
    }
    
    private void SetTurn(bool IamNext)
    {
        if (!IsServer()) return;
      
        currentTry = 0;
        myTurn = IamNext;
        Debug.Log("It's " + (myTurn ? "my Turn" : "the enemys Turn") + " Turn!");

        BallManager.instance.SetPositionToBallHolder(myTurn);

        networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.All, !myTurn);

        // TODO: Show the player some indication it's his turn!
    }

    public int GetPlayerPoints()
    {
        return networkObject != null ? networkObject.IsServer ? networkObject.hostPoints : networkObject.clientPoints : 0;
    }

    public int GetEnemyPoints()
    {
        return networkObject != null ? networkObject.IsServer ? networkObject.clientPoints : networkObject.hostPoints : 0;
    }

    public float GetCurrentPlayedTime()
    {
        return networkObject != null ? networkObject.playedTime : 0f;
    }

    // RPC, do not call directly!
    public override void PlayerTurn(RpcArgs args)
    {
        if (IsServer()) return;

        myTurn = args.GetNext<bool>();

        // TODO: Show the player some indication it's his turn!
    }

    // RPC, do not call directly!
    public override void GameOver(RpcArgs args)
    {
        if (IsServer()) return;

        // TODO: Show some end screen etc...
        throw new System.NotImplementedException();
    }

    public void BallWasGrabbed()
    {
        // TODO: this is dead right now...
        //currentTry++;
    }
}