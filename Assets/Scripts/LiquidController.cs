using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


/// <summary>
/// Idee: Generate sample points around edge of cup
/// If one or more is/are under the y-world-height of the liquid 
/// If there are more under the y-w-h interpolate direction (leap)
/// If, then activate the second partical system (strahl)
/// and change direction
/// </summary>
[RequireComponent(typeof(CupController))]
public class LiquidController : MonoBehaviour
{
     CupController cupController;

    // CupSesific
    [SerializeField] private float cupHeight;
    [SerializeField] private float radius;

    // Logic
    [SerializeField] private float numberOutlets;
    
    // Start is called before the first frame update
    void Start()
    {
        cupController = GetComponent<CupController>();

        Debug.Log("height: " + cupHeight + " numberOutlets: " + numberOutlets);

        GenerateOutlets();
    }

    private void GenerateOutlets()
    {
        float numberOutlets, radius;

        Vector3 baseVec = gameObject.transform.position + new Vector3(0,cupHeight,0);


    }

}
