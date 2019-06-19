using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public int audioCountUp = 0;

 


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

            AudioManager.instance.Play("BallHitGround");
        }

        else if (other.gameObject.tag.Equals("Wall"))
        {
            GameManager.instance.BallFallBeside();

            AudioManager.instance.Play("BallHitWall");
        }


        else if (other.gameObject.tag.Equals("Table"))
        { 
            GameManager.instance.BallFallBeside();
            
            if (audioCountUp == 0)
            {
                AudioManager.instance.Play("BallHitTable1");
                audioCountUp++;
            }
            
            else if (audioCountUp == 1)
            {
                AudioManager.instance.Play("BallHitTable2");
                audioCountUp++;
            }

            else if (audioCountUp == 2)
            {
                AudioManager.instance.Play("BallHitTable3");
                audioCountUp = 0;
            }

        }
        
        else if (other.gameObject.tag.Equals("Counter"))
        { 
            GameManager.instance.BallFallBeside();

            AudioManager.instance.Play("BallHitCounter");



        }
        
    } 


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Cup"))
        {
            other.gameObject.GetComponentInParent<CupController>().DeactivateTheCup();
            GameManager.instance.BallFallInCup();

        
            AudioManager.instance.Play("BallHitCup");

        
        }
    }

    public void WasTaken()
    {
        BallManager.instance.BallIsGrabbed();

        AudioManager.instance.Play("TakeBall");
    }
}