using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public GameManager Manager;

    // Update is called once per frame
    void Update()
    {
        //GameManager has to be activate because of some other infected this GameObject
        Manager.gameObject.SetActive(true);
    }
}
