using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

/*           0 1 2 3 4 5 6   
 *       0         +
 *       1       +   +
 *       2     +   +   +
 *       3   +   +   +   +
 */





public class CupBundleController2 : MonoBehaviour
{
    [SerializeField] private GameObject cupPrefab;

    // TODO: Get from GameManager?
    // #maxCups

    private int numRows;

    private float radius =.1f;

    // Start is called before the first frame update
    void Start()
    {
        CreateCups();
    }




    void CreateCups()
    {
        numRows = 4;
        float row = 0;

        for (int y = 0; y < numRows; y++)
        {
            for (int x = -y; x <= y; x+=2)
            {
                Debug.Log("x: " +x + " y: " + y);

                Vector3 pos =  new Vector3( x * radius, 0f, y * radius);
                GameObject gamaObject = Instantiate(cupPrefab, transform.position + pos , Quaternion.identity, transform) as GameObject;
            }
        }

    }

}
//Instantiate(cupPrefab, new Vector3(-i * radius, 0f, radius), Quaternion.identity, transform);

