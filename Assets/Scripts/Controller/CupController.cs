using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{

    public bool fliped = false;
    private float _criticalAngle = 45.0f;

    public float Angle { get; set; }
    public bool Empty { get; internal set; }

    [SerializeField] private int cupPositionInGroup;

    // Update is called once per frame
    void Update()
    {
        // get angle in Range [0, 180]
        Angle = Vector3.Angle(Vector3.up, transform.forward);

        if (Angle > _criticalAngle)
            fliped = true;
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
        return cupPositionInGroup;
    }
}



