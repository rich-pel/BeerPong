using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTex : MonoBehaviour
{
    private Renderer _myRenderer;
    private float _blendAmount;
    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0.01f; // [0.1 : 1]
        _myRenderer = GetComponent<Renderer>();
        if (_myRenderer == null)
        {
            Debug.Log("Renderer Component is missing");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _blendAmount = Time.time * _speed;

        Debug.Log(_blendAmount);

        if (_blendAmount < 1)
        {
            _myRenderer.sharedMaterial.SetFloat("_Blend", _blendAmount);
        }
        else
        {
            this.enabled = false;
        }
    }
}
