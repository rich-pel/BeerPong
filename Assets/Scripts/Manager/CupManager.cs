﻿using BeardedManStudios.Forge.Networking.Generated;
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

    public bool AllCupsFound { get; private set; }
    [SerializeField] private CupBundleController playersCupBundle;
    [SerializeField] private CupBundleController enemysCupBundle;
    public const float CupHeight = 0.11f;
    public const float CupRadius = 0.04f;


    public void InitClientCups()
    {
        AllCupsFound = playersCupBundle.InitClient(true) && enemysCupBundle.InitClient(false);
    }  

    public void InitCups()
    {
        playersCupBundle.Init(true);
        enemysCupBundle.Init(false);
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

    public void SetCupsOwnership(bool IOwnThem)
    {
        playersCupBundle.SetCupsOwnership(IOwnThem);
        enemysCupBundle.SetCupsOwnership(IOwnThem);
    }

    // Gets also called by Client!
    public bool AmIOwnerOfCups()
    {
        return playersCupBundle.AmIOwnerOfCups() && enemysCupBundle.AmIOwnerOfCups();
    }

    // Gets also called by Client!
    public void ResetCups(bool activeOnly)
    {
        playersCupBundle.ResetCups(activeOnly);
        enemysCupBundle.ResetCups(activeOnly);
    }

    // Gets also called by Client!
    public void SyncCups(bool sync)
    {
        playersCupBundle.SyncCups(sync);
        enemysCupBundle.SyncCups(sync);
    }

    // set values for controllers
    public void SetParameter(Mesh cupMesh)
    {
        // Use Boundingbox of Mesh...
        // for debugging:
        //Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        //Debug.Log(mesh.name
        //          + "\n center: " + mesh.bounds.center
        //          + "\n extents: " + mesh.bounds.extents
        //          + "\n max: " + mesh.bounds.max
        //          + "\n min: " + mesh.bounds.min
        //          + "\n size: " + mesh.bounds.size);

        //foreach (var vert in mesh.vertices)
        //{
        //    Debug.Log(vert);
        //}
        // Debug.Log("The global scale of one Cup is: " + globalScale(this.transform));
    }
}