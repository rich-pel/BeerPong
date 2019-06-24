using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CupController : MonoBehaviour
{
    public CupBundleController father;
    private Vector3 homePosition;
    private Rigidbody body;

    void Start()
    {
        homePosition = transform.position;
        body = GetComponent<Rigidbody>(); // no check required because of RequireComponent
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        transform.position = homePosition;
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        gameObject.SetActive(true);
    }
}



