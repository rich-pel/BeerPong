using System.Collections.Generic;
using System.Collections;
using UnityEngine;

        
/// <summary>
/// Idea: Generate sample points around upper edge of cup;
/// If one or more is/are under the y-world-height of the liquid -> spilling in correct direction
/// If there are more under the y-w-h interpolate direction (leap) -> spilling in coorect direction
/// </summary>
public class LiquidController : MonoBehaviour
{
    [SerializeField] private GameObject childFoam;
    [SerializeField] private BlendTex formBlendTex;
    [SerializeField] private GameObject childSpillage;
    [SerializeField] private int numberOutlets;
    private List<GameObject> Outlets = new List<GameObject>();

    // CupSpecific
    private float _cupHeight;
    private float _radius;

    // spillage
    private Vector3 _target;

    // foam
    private float _angleX;
    private float _angleZ;
    private Vector3 _initScale;

    private int _fillLevel = 100;

    // Start is called before the first frame update
    private void Start()
    {
        _cupHeight = 0.11f; //CupManager.CupHeight;
        _radius = 0.04f;

        _initScale = childFoam.transform.localScale;

        GenerateOutlets(numberOutlets);
    }

    private void OnEnable()
    {
        _fillLevel = 100;
        childFoam.SetActive(true);
        formBlendTex.Restart();
    }


    private void GenerateOutlets(int numOutlets)
    {
        Outlets.Clear();
       
        for (int i = 0; i < numOutlets; i++)
        {
            GameObject dummy = new GameObject("Point" + i.ToString());
            dummy.transform.parent = transform;

            float cornerAngle = 2f * Mathf.PI / (float)numOutlets * i;
            // bei rot.x = -90*
            // Vector3 pos = new Vector3(Mathf.Cos(cornerAngle) * _radius, Mathf.Sin(cornerAngle) * _radius, _cupHeight); //+ basePos;
            // ohne rot
            Vector3 pos = new Vector3( Mathf.Cos(cornerAngle) * _radius, 
                                       _cupHeight, 
                                       Mathf.Sin(cornerAngle) * _radius); //+ basePos;

            dummy.transform.localPosition = pos;
            Outlets.Add(dummy);
        }
    }

    void Update()
    {
        /*

        // ################# F O A M #################
        // Orientation
        childFoam.transform.up = Vector3.up; 

        // Scale
        _angleX = Mathf.Abs( Vector3.Angle(Vector3.up, transform.forward)); // scale
        _angleZ = Mathf.Abs( Vector3.Angle(Vector3.up, transform.right)); // scale

        Debug.Log("X Angle: " + _angleX);
        Debug.Log("Z Angle: " + _angleZ);

        // etwas von hinten durch die Brust ins Auge machen
        //Vector3 _scale = childFoam.transform.localScale;

        Vector3 _scale = _initScale;
        _scale.x = Mathf.Sin(_angleX); // / _initScale.x;
        _scale.z = Mathf.Cos(_angleZ); // / _initScale.z;

        if (_scale.x <= 45 && _scale.y <= 45)
        {
            childFoam.transform.localScale = _scale;
            childFoam.SetActive(true);
        }
        else
        {
            childFoam.SetActive(false);
        }

        Debug.Log("X Scale: " + _scale.x);
        Debug.Log("Z Scale: " + _scale.z);


    */


        // ################# Spillage ################# 
        int myCount = 0;
        _target = Vector3.zero;
        foreach (GameObject outlet in Outlets)
        {
            // foam bleibt immer in der mitte des cups
            if (outlet.transform.position.y < childFoam.transform.position.y)
            {
                _target += outlet.transform.position;
                myCount++;
            }
        }

        Debug.Log("Count:" + myCount + " target: " + _target);


        if (myCount > 1)
        {
            if (_fillLevel > 0) { 
                childSpillage.SetActive(true);

                // pq = q-p
                childSpillage.transform.forward = _target - childSpillage.transform.position;
                _fillLevel -= myCount;
            }
            else
            {
                childSpillage.SetActive(false);
            }
        }
        else
        {
            childSpillage.SetActive(false);
        }




    }
}
