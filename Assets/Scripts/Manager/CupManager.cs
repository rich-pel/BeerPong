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

    public void DeactivateACoupInAGroup(int cupNumber, bool playersCup)
    {
        if (playersCup)
            DeactivateACoupInABundle(cupNumber, playersCupBundle);
        else
            DeactivateACoupInABundle(cupNumber, enemysCupBundle);
            
    }

    private static void DeactivateACoupInABundle(int cupNumber, CupBundleController cupBundleController)
    {
        cupBundleController.DeactivateCupAt(cupNumber);
    }

    public void ReGroupACupGroup(bool playersCup)
    {
        if (playersCup)
            ReGroupACupBundle(playersCupBundle);
        else
            ReGroupACupBundle(enemysCupBundle);
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