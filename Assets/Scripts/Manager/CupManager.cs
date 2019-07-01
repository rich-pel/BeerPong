using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    #region Singleton

    public static CupManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    [SerializeField] private CupBundleController playersCupBundle;
    [SerializeField] private CupBundleController enemysCupBundle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool IsMyCup(CupController Cup)
    {
        if (Cup.father == null)
        {
            Debug.LogError("Cup '" + Cup + "' does not have a father (CupBundleController)!");
            return false;
        }

        return Cup.father == playersCupBundle;
    }

    //public void DeactivateCup(CupController Cup)
    //{
    //    if (Cup == null)
    //    {
    //        Debug.LogError("Tried to deactivate NULL Cup!");
    //        return;
    //    }

    //    Cup.gameObject.SetActive(false);
    //}

    public void ResetAllCups()
    {
        playersCupBundle.ResetAllCups(false);
        enemysCupBundle.ResetAllCups(false);
    }

    public void StandActiveCupsBackToOringPos(bool myTurn)
    {
        if (myTurn)
            enemysCupBundle.ResetAllCups(true);
        else
            playersCupBundle.ResetAllCups(true);
    }
}