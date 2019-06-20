using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : GameManagerBehavior
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private bool myTurn = false;
    [SerializeField] private int MaxPoints = 10;
    private bool bInit = false;

    [SerializeField] private int maxTries = 3;
    private int currentTry = 0;

    // Start is called before the first frame update
    void Init()
    {
        if (networkObject.IsServer)
        {
            myTurn = true; // host always starts
            networkObject.hostPoints = 0;
            networkObject.clientPoints = 0;
            networkObject.playedTime = 0;
            networkObject.MaxPoints = MaxPoints;
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

        if (!bInit)
        {
            Init();
            bInit = true;
        }

        if (networkObject.IsServer)
        {
            if (currentTry >= maxTries && !BallManager.instance.BallIsInAction())
            {
                SetTurn(!checkIsMyTurn());
                currentTry = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space Hit: " + currentTry);
                currentTry++; 
            }

            networkObject.playedTime += Time.deltaTime;
        }
    }

    private void switchTurn()
    {
        myTurn = !myTurn;
        BallManager.instance.SetPositionToBallHolder(checkIsMyTurn());
        currentTry = 0;
        

    }

    private bool checkIsMyTurn()
    {
        return currentTry < maxTries;
    }

    public bool IsServer()
    {
        return networkObject != null ? networkObject.IsServer : false;
    }

    public void BallFellInCup(int cupPosition)
    {
        if (networkObject == null || !networkObject.IsServer) return;

        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fell In Cup");

        CupManager.instance.DeactivateACoupInAGroup(cupPosition, checkIsMyTurn());
        
        //Here has also to happend the update for the points and so on...
        if (checkIsMyTurn())
        {
            networkObject.hostPoints++;
        }
        else
        {
            networkObject.clientPoints++;
        }

        currentTry++;
        //SetTurn(!checkIsMyTurn());
    }

    public void BallFellBeside()
    {
        if (networkObject == null || !networkObject.IsServer) return;

        //no counting for the points
        //SetTurn(!checkIsMyTurn());
        currentTry++;
    }

    private void SetTurn(bool IamNext)
    {
        if (networkObject == null || !networkObject.IsServer) return;

        myTurn = IamNext;
        BallManager.instance.SetPositionToBallHolder(myTurn);

        networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.All, !myTurn);

        // TODO: Show the player some indication it's his turn!
    }

    public int GetPlayerPoints()
    {
        return networkObject != null ? networkObject.clientPoints : 0;
    }

    public int GetEnemyPoints()
    {
        return networkObject != null ? networkObject.hostPoints : 0;
    }

    public float GetCurrentPlayedTime()
    {
        return networkObject != null ? networkObject.playedTime : 0f;
    }

    public int GetMaxPoints()
    {
        return networkObject != null ? networkObject.MaxPoints : 0;
    }

    // RPC, do not call directly!
    public override void PlayerTurn(RpcArgs args)
    {
        if (networkObject.IsServer) return;

        myTurn = args.GetNext<bool>();

        // TODO: Show the player some indication it's his turn!
    }

    // RPC, do not call directly!
    public override void GameOver(RpcArgs args)
    {
        if (networkObject.IsServer) return;

        // TODO: Show some end screen etc...
        throw new System.NotImplementedException();
    }

    public void BallWasGrabbed()
    {
        // TODO: this is dead right now...
        //currentTry++;
    }

    public void StartRound()
    {
        CupManager.instance.SetAllCupsActive();
    }
    
}