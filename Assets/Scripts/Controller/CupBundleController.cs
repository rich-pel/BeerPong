using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBundleController : MonoBehaviour
{

    [SerializeField] private List<CupController> cupsInGroup;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DeactivateCupAt(int cupNumber)
    {
        CupController cupController = GetCupWithNumber(cupNumber);
        if (cupController != null) 
        {
            cupController.DeactivateTheCup();
        }
    }

    private CupController GetCupWithNumber(int cupNumber)
    {
        foreach (CupController cupController in cupsInGroup)
        {
            if (cupController.GetCupPositionInGroup() == cupNumber)
            {
                return cupController;
            }
        }
        return null;
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

    public void ActivateAllCups()
    {
        foreach (CupController cup in cupsInGroup)
        {
            cup.ActivateTheCup();
        }
    }
}