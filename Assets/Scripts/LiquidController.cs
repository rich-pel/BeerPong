using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CupController))]
public class LiquidController : MonoBehaviour
{
     CupController cupController;

    // Start is called before the first frame update
    void Start()
    {
        cupController = GetComponent<CupController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
