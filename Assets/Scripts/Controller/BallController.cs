using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        //Maybe just if the collision enter the ballFallBeside
        if (other.gameObject.tag.Equals("Ground") || other.gameObject.tag.Equals("Wall") ||
            other.gameObject.tag.Equals("Table"))
        {
            GameManager.instance.BallFallBeside();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("CupHitArea"))
        {
            other.gameObject.GetComponentInParent<CupController>().DeactivateTheCup();
            GameManager.instance.BallFallInCup();
        }
    }

    public void WasTaken()
    {
        BallManager.instance.BallIsGrabbed();
    }
}