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

    public void DeactivateCoup(int cupNumber, bool playersCup)
    {
        if (playersCup)
            playersCupBundle.DeactivateCupAt(cupNumber);
        else
            enemysCupBundle.DeactivateCupAt(cupNumber);
    }

    public void ResetAllCups()
    {
        playersCupBundle.ResetAllCups();
        enemysCupBundle.ResetAllCups();
    }
}