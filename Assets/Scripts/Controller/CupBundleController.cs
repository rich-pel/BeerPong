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
    
    public void DeactivateCup(CupController Cup)
    {
        if (!Cup)
        {
            Debug.LogError("Tried to deactivate Cup NULL!");
            return;
        }
        
        if (!cupsInGroup.Contains(Cup))
        {
            Debug.LogError("Tried to deactivate Cup '"+Cup+"' not present in our group!");
            return;
        }
        
        Cup.Deactivate();
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

    public void ResetAllCups()
    {
        foreach (CupController cup in cupsInGroup)
        {
            cup.Reset();
        }
    }

    public void StandUpCupsAgain()
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
            {
                cup.ReturnToOriginPosition();
            }
        }
    }
}