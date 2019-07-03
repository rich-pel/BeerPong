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
    [SerializeField] private GameObject childForm;
    [SerializeField] private GameObject childSpillage;

    [SerializeField] private int numberOutlets;
    private List<GameObject> Outlets = new List<GameObject>();

    // CupSpecific
    private float _cupHeight;
    private float _radius;

    private Vector3 _target;



    // Start is called before the first frame update
    private void Start()
    {
        _cupHeight = CupManager.CupHeight / 2;
        _radius = CupManager.CupRadius;

        GenerateOutlets(numberOutlets);
    }

    private void GenerateOutlets(int numOutlets)
    {
        Outlets.Clear();
       
        for (int i = 0; i < numOutlets; i++)
        {
            GameObject dummy = new GameObject("Point" + i.ToString());
            dummy.transform.parent = transform;

            float cornerAngle = 2f * Mathf.PI / (float)numOutlets * i;
            Vector3 pos = new Vector3(Mathf.Cos(cornerAngle) * _radius, Mathf.Sin(cornerAngle) * _radius, _cupHeight); //+ basePos;
            dummy.transform.localPosition = pos;

            Outlets.Add(dummy);
        }
    }

    void Update()
    {

        int count = 0;
        _target = Vector3.zero;

        foreach (GameObject outlet in Outlets)
        {
            if (outlet.transform.position.y > transform.position.y)
            {
                _target += outlet.transform.position;
                count++;
            }
        }


        // Manage Spillage
        
        if (count < 1)
        {
            // child Spillage
            childSpillage.SetActive(false);
        }
        else
        {
            // Spillage
            childSpillage.SetActive(true);
            _target *= count;
            childSpillage.transform.LookAt(_target);

            if (heightFoam <= 0)
            {

            }
        }

        // pose of liquid parts
        childForm.transform.LookAt(childForm.transform.position + Vector3.up);
    }

    // TODO: Remove to CupManager / Some Namespace
    /// <summary>
    /// Returns the scaling through all parents
    /// </summary>
    /// <param name="requestedTransform"></param>
    /// <returns></returns>
    private Vector3 globalScale(Transform requestedTransform)
    {
        Vector3 _scale = requestedTransform.localScale;

        while (requestedTransform.transform.parent != null)
        {
            //_scale *= requestedTransform.localScale;
            _scale = Vector3.Scale(_scale, requestedTransform.localScale);
            // next recursive level
            requestedTransform = requestedTransform.transform.parent;
        }

        return _scale;
    }
}
