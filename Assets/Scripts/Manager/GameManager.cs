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

    // Start is called before the first frame update
    void Init()
    {
        if (networkObject.IsServer)
        {
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
            bInit = false;
        }

        if (networkObject.IsServer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerTurn(myTurn);
            }

            networkObject.playedTime += Time.deltaTime;
        }
    }

    public bool IsServer()
    {
        return networkObject != null ? networkObject.IsServer : false;
    }

    public void BallFellInCup()
    {
        if (networkObject == null) return;

        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fell In Cup");

        //Here has also to happend the update for the points and so on...
        if (myTurn)
        {
            networkObject.hostPoints++;
        }
        else
        {
            networkObject.clientPoints++;
        }

        PlayerTurn(!myTurn);
    }

    public void BallFellBeside()
    {
        //no counting for the points
        PlayerTurn(!myTurn);
    }

    private void PlayerTurn(bool turn)
    {
        BallManager.instance.SetPositionToBallHolder(turn);
        myTurn = turn;

        // inform client about turn change
        if (networkObject != null)
        {
            networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.All, myTurn);
        }
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

    bool RPCAuthorityCheck()
    {
        if (networkObject.IsServer)
        {
            Debug.LogWarning("Some Client tried to call an RPC method on us... how bold!");
            return false;
        }
        return true;
    }

    // RPC, do not call directly!
    public override void PlayerTurn(RpcArgs args)
    {
        if (!RPCAuthorityCheck()) return;

        myTurn = !args.GetNext<bool>();
    }

    // RPC, do not call directly!
    public override void GameOver(RpcArgs args)
    {
        if (!RPCAuthorityCheck()) return;

        // TODO: Show some end screen etc...
        throw new System.NotImplementedException();
    }
}