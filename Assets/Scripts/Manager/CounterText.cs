using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CounterText : MonoBehaviour
{
	public Text redPointsText;
	public Text bluePointsText;


    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        redPointsText.text = " "+ GameManager.instance.GetRedPoints();
		bluePointsText.text = " "+ GameManager.instance.GetBluePoints();
    }
}
