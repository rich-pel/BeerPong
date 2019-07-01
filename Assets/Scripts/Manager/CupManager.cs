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



    public void DeactivateACoupInAGroup(int cupNumber, bool playersCup)
    {
        DeactivateACoupInABundle(cupNumber, playersCup ? playersCupBundle : enemysCupBundle);
    }

    private static void DeactivateACoupInABundle(int cupNumber, CupBundleController cupBundleController)
    {
        cupBundleController.DeactivateCupAt(cupNumber);
    }

    public void ReGroupACupGroup(bool playersCup)
    {
        ReGroupACupBundle(playersCup ? playersCupBundle : enemysCupBundle);
    }

    private void ReGroupACupBundle(CupBundleController cupBundle)
    {

        int activeCups = cupBundle.GetNumberOfActiveCups();
        throw new NotImplementedException();
    }

    public void SetAllCupsActive()
    {
        playersCupBundle.ActivateAllCups();
        enemysCupBundle.ActivateAllCups();
    }

   
}