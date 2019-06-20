using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class     CupController : MonoBehaviour
{

    [SerializeField] private int cupPosistionInGroup;
//    public int CupNumberInGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeactivateTheCup()
    {
        //Cup Group should be informed that this cup is not available anymore, based on the cup number?
        gameObject.SetActive(false);
    }


    public void ActivateTheCup()
    {
        gameObject.SetActive(true);
        
    }
    public void SwitchActiveStatus()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
    
    public int GetCupPositionInGroup()
    {
        return cupPosistionInGroup;
    }
}



