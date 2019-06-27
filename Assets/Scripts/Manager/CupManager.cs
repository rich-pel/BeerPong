using System;
using System.Collections;
using System.Collections.Generic;
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

    public void ResetAllCups()
    {
        playersCupBundle.ResetAllCups();
        enemysCupBundle.ResetAllCups();
    }

    public void StandActiveCupsBackToOringPos(bool notMyTurnAnymore)
    {
        if (notMyTurnAnymore)
            playersCupBundle.StandUpCupsAgain();
        else
            enemysCupBundle.StandUpCupsAgain();
    }
}