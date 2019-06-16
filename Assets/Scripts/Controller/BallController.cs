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
        if (other.gameObject.tag.Equals("Ground"))
        {

            GameManager.instance.BallFallBeside();

            FindObjectOfType<AudioManager>().Play("Ballhitground");
        }

        else if (other.gameObject.tag.Equals("Wall"))
        {
            GameManager.instance.BallFallBeside();

            FindObjectOfType<AudioManager>().Play("Ballhitwall");
        }

        else if (other.gameObject.tag.Equals("Table"))
        { 
            GameManager.instance.BallFallBeside();

            FindObjectOfType<AudioManager>().Play("Ballhittable");

        }
        //we need a counter 
        else if (other.gameObject.tag.Equals("Counter"))
        { 
            GameManager.instance.BallFallBeside();

            FindObjectOfType<AudioManager>().Play("Ballhitcounter");

        }
        
    } 


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("CupHitArea"))
        {
            other.gameObject.GetComponentInParent<CupController>().DeactivateTheCup();
            GameManager.instance.BallFallInCup();

            FindObjectOfType<AudioManager>().Play("Ballhitcup");

        }
    }

    public void WasTaken()
    {
        BallManager.instance.BallIsGrabbed();

        FindObjectOfType<AudioManager>().Play("Takeball");
    }
}