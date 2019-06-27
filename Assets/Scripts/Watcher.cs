using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public GameManager Manager;

    // Update is called once per frame
    void Update()
    {
        Manager.gameObject.SetActive(true);
    }
}
