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
    private Vector3 _target;
    private float _angleX;
    private float _angleY;


    private float _fillLevel = 100f;

    // Start is called before the first frame update
    private void Start()
    {
        _cupHeight = 0.11f; //CupManager.CupHeight;
        _radius = 0.04f;

        GenerateOutlets(numberOutlets);
    }

    private void OnEnable()
    {
        _fillLevel = 100f;
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
            Vector3 pos = new Vector3(Mathf.Cos(cornerAngle) * _radius, _cupHeight, Mathf.Sin(cornerAngle) * _radius); //+ basePos;
            dummy.transform.localPosition = pos;

            Outlets.Add(dummy);
        }
    }

    void Update()
    {

        // Get state of cup
        int count = 0;
        _target = Vector3.zero;
        foreach (GameObject outlet in Outlets)
        {
            // foam bleibt immer in der mitte des cups
            if (outlet.transform.position.y > childFoam.transform.position.y )
            {
                _target += outlet.transform.localPosition;
                count++;
            }
        }

        // hanlde Foam
        childFoam.transform.LookAt(Vector3.up); // Orientation
        _angleX = Vector3.Angle(Vector3.right, transform.forward); // scale
        _angleY = Vector3.Angle(Vector3.up, transform.forward); // scale

        Debug.Log("X: " + _angleX);
        Debug.Log("Y: " + _angleY);

        // etwas von hinten durch die Brust und durchs ins Auge machen
        Vector3 _scale = childFoam.transform.localScale;
        _scale.x = Mathf.Cos(_angleX); // /0.4f;
        _scale.y = Mathf.Cos(_angleY); // /0.4f;
        childFoam.transform.localScale = _scale;


        // handle Spillage 
        if(count < 1 && _fillLevel > 0)
        {
            childSpillage.SetActive(true);
            childSpillage.transform.forward = _target.normalized;
            _fillLevel -= count;
        }
        else
        {
            // Spillage
            childSpillage.SetActive(false);
        }
    }
}
