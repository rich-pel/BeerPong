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

        if (currentTry >= maxTries && !BallManager.instance.BallIsInAction())
        {
            switchTurn();
        }
        
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetTurn(!checkIsMyTurn());
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space Hit: " + currentTry);
                BallManager.instance.SetPositionToBallHolder(checkIsMyTurn());
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
        if (networkObject == null) return;

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

        SetTurn(!checkIsMyTurn());
    }

    public void BallFellBeside()
    {
        //no counting for the points
        SetTurn(!checkIsMyTurn());
    }

    private void SetTurn(bool IamNext)
    {
//        myTurn = IamNext;
        if (networkObject.IsServer)
        {
            BallManager.instance.SetPositionToBallHolder(checkIsMyTurn());
        }

        if (networkObject != null)
        {
            // Inform everyone about turn change
            networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.All, !checkIsMyTurn());
        }

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
        currentTry++;
    }
}