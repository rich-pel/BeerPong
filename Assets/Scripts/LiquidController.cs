using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
 TODO: Start: Scale radius, Scale height // neu
 TODO          Download for Foam Texture
 
*/

// Beschreibung des Cups
/*
 * Der Cup hat nen Meshfilter, Meshrenderer, collider
 * und nen CupController und nen LiquidController als Componenten
 * Als Kinder mit Foam (particle system attached) und Spillage (particle system) 
 *
 * vlt Foam als plane - da gibts aber schwierigkeiten mit den rändern des bechers, da diese sich verschieben
 *
 * alternative: nur Shader der blubbert, mit werten für die neigung. Der wird auf eine Plane gemacht und dann wird die plan je nach transform gedreht
 * In dem shader gibt es einen kreis, alles andere ist transparent, der kreis wird mit zunehmendem abstand kleiner
 * Schwierigkeit: soll jeder sein eigenes material bekommen? 
 *
 */

        
/// <summary>
/// Idea: Generate sample points around edge of cup
/// If one or more is/are under the y-world-height of the liquid 
/// If there are more under the y-w-h interpolate direction (leap)
/// If, then activate the second particle system (strahl)
/// and change direction
/// </summary>
[RequireComponent(typeof(CupController))]
public class LiquidController : MonoBehaviour
{
    private CupController _cupController;

    // CupSpecific
    [SerializeField] private float editorCupHeight;
    [SerializeField] private float editorRadius;

    [SerializeField] private float editorMaxFormHeight; 
    [SerializeField] private float editorBaseHeightSpillage; 

    // Logic
    [SerializeField] private int numberOutlets;

    private List<GameObject> Outlets;


    // sollte 
    [SerializeField] private GameObject ChildForm;
    [SerializeField] private GameObject ChildSpillage;


    private Vector3 target;

    private float heightFoam;
    private float baseHeightSpillage;


    // Start is called before the first frame update
    private void Start()
    {
        // Use Boundingbox of Mesh...
        // for debugging:
        //Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        //Debug.Log(mesh.name
        //          + "\n center: " + mesh.bounds.center
        //          + "\n extents: " + mesh.bounds.extents
        //          + "\n max: " + mesh.bounds.max
        //          + "\n min: " + mesh.bounds.min
        //          + "\n size: " + mesh.bounds.size);

        //foreach (var vert in mesh.vertices)
        //{
        //    Debug.Log(vert);
        //}
        // Debug.Log("The global scale of one Cup is: " + globalScale(this.transform));

        Refill();
        _cupController = GetComponent<CupController>();

        Outlets = new List<GameObject>();
        GenerateOutlets(numberOutlets);
    }

    public void Refill()
    {
        // height = 0.75 * globalScale(this.transform);
        heightFoam = editorCupHeight * editorMaxFormHeight; // 0.8
        baseHeightSpillage = editorCupHeight * editorBaseHeightSpillage; //0.4
    }



    private void GenerateOutlets(int numOutlets)
    {
        Outlets.Clear();

        // because its a child of this (cup) not necessary
        // Vector3 basePos = gameObject.transform.position + this.gameObject.transform.forward * cupHeight;


        for (int i = 0; i < numOutlets; i++)
        {
            // erstelle neues GameObject 
            GameObject dummy = new GameObject("Point" + i.ToString());
            // uebernehme die Transformation vom parent (hier hauptsächlich die rotation um -90° in x)
            dummy.transform.parent = transform;

            float cornerAngle = 2f * Mathf.PI / (float)numOutlets * i;
            Vector3 pos = new Vector3(Mathf.Cos(cornerAngle) * editorRadius, Mathf.Sin(cornerAngle) * editorRadius, editorCupHeight); //+ basePos;
            dummy.transform.localPosition = pos;
            
            // behalte eine Referenz
            Outlets.Add(dummy);
        }
    }

    void Update()
    //void FixedUpdate()
    {

        // calculate direction of spillage particle system
        int count = 0;
        target = Vector3.zero;

        foreach (GameObject outlet in Outlets)
        {
            if (outlet.transform.position.y > transform.position.y)
            {
                target += outlet.transform.position;
                count++;
            }
        }


        // Manage height
        //
        //if (count < 1)
        //{
        //    // child Spillage
        //    Spillage.SetActive(false);
        //}
        //else
        //{
        //    // Spillage
        //    Spillage.SetActive(true);
        //    target *= count;
        //    Spillage.transform.LookAt(target);

        //    // Foam
        //    heightFoam -= 0.1f * count;

        //    if (heightFoam <= 0)
        //    {
        //        _cupController.Empty = true;
        //        // deactivates the gameObject 
        //        _cupController.DeactivateTheCup();

        //    }
        //}

        // pose of liquid parts
        ChildForm.transform.LookAt(ChildForm.transform.position + Vector3.up); 
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
