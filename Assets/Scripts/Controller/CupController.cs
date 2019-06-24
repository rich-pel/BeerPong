using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CupController : MonoBehaviour
{
    [SerializeField] private int cupPosistionInGroup;
    private Vector3 homePosition;
    private Rigidbody body;

    void Start()
    {
        homePosition = transform.position;
        body = GetComponent<Rigidbody>();
    }

    public void Deactivate()
    {
        //Cup Group should be informed that this cup is not available anymore, based on the cup number?
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        transform.position = homePosition;
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        gameObject.SetActive(true);
    }
    
    public int GetCupPosition()
    {
        return cupPosistionInGroup;
    }
}



