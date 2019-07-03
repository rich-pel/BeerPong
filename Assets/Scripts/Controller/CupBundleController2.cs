using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

/*       y
 *     x    -3 -2 -1 0  1  2  3
 *       0           +
 *       1         +    +
 *       2      +    +     +
 *       3   +     +    +     +
 */


public class CupBundleController2 : MonoBehaviour
{
    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private float radius = 0.7f;

    // TODO: Get from GameManager?
    // #maxCups

    private int _numRows = 4;
    private float _gab = 0.011f;

    private float _distanceX;
    private float _distanceY;

    // Start is called before the first frame update
    void Start()
    {
        _distanceY = radius + _gab;
        _distanceX = radius / 2 + _gab;

        // get radius from somewhere

        CreateCups();
    }

    /// <summary>
    /// Instantiates Cups in diamond formation
    /// </summary>
    void CreateCups()
    {

        int _count=0;

        for (int y = 0; y < _numRows; y++)
        {
            for (int x = -y; x <= y; x+=2)
            {
                // local position
                // Vector3 pos = new Vector3(x * _distance /2, 0f, y * _distance);
                Vector3 pos = new Vector3(x * _distanceX , 0f, y * _distanceY);

                // Instance settings
                GameObject myGO = Instantiate(cupPrefab, transform);
                myGO.name = "Cup" + _count;
                myGO.transform.localPosition = pos;

                _count++;
                // TODO: Lass das den copController machen: myGO.GetComponent<CupController>().father = this.transform; 
            }
        }

    }

}
//GameObject gamaObject = Instantiate(cupPrefab, transform.position + pos , Quaternion.identity, transform) as GameObject;

