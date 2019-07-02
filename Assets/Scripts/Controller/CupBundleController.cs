using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBundleController : MonoBehaviour
{
    [SerializeField] private List<CupController> cupsInGroup;

    void Start()
    {
        foreach (CupController cup in cupsInGroup)
        {
            cup.father = this;
        }
    }

    public int GetNumberOfActiveCups()
    {
        int cupsActive = 0;

        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
                cupsActive++;
        }
        
        return cupsActive;
    }

    public void SetCupsOwnership(bool IOwnThem)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
            {
                cup.SetOwnership(IOwnThem);
            }
        }
    }

    public bool AmIOwnerOfCups()
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy && !cup.networkObject.IsOwner) return false;
        }
        return true;
    }

    public void ResetCups(bool activeOnly)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (activeOnly && !cup.gameObject.activeInHierarchy) continue;

            //cup.SetActive(true);
            cup.Reset();
        }
    }

    public void SyncCups(bool sync)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
            {
                cup.SetSync(sync);
            }
        }
    }
}