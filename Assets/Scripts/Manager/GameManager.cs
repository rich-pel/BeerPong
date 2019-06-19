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

    // Start is called before the first frame update
    void Start()
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
        if (!networkObject.IsServer) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerTurn(myTurn);
        }

        networkObject.playedTime = networkObject.playedTime + Time.deltaTime;
    }

    public bool IsServer()
    {
        return networkObject.IsServer;
    }

    public void BallFellInCup()
    {
        //Zugehörige Gruppe
        //Aktueller Becher
        //Becher muss verschwinden
        //Punkte Stand muss erhöht werden
        //Ball muss position wechseln
        Debug.Log("In Ball Fell In Cup");
        //Here has also to happend the update for the points and so on...
        if (myTurn)
            networkObject.clientPoints++;
        else
            networkObject.hostPoints++;
        PlayerTurn(myTurn);
    }

    public void BallFellBeside()
    {
        //no counting for the points
        PlayerTurn(myTurn);
    }

    private void PlayerTurn(bool turn)
    {
        BallManager.instance.SetPositionToBallHolder(turn);
        myTurn = turn;

        // inform client about turn change
        networkObject.SendRpc(RPC_PLAYER_TURN, Receivers.All, myTurn);
    }

    public int GetPlayerPoints()
    {
        return networkObject.clientPoints;
    }
    public int GetEnemyPoints()
    {
        return networkObject.hostPoints;
    }

    public float GetCurrentPlayedTime()
    {
        return networkObject.playedTime;
    }

    public int GetMaxPoints()
    {
        return networkObject.MaxPoints;
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

    public override void PlayerTurn(RpcArgs args)
    {
        if (!RPCAuthorityCheck()) return;

        myTurn = !args.GetNext<bool>();
    }

    public override void GameOver(RpcArgs args)
    {
        if (!RPCAuthorityCheck()) return;

        // TODO: Show some end screen etc...
        throw new System.NotImplementedException();
    }
}