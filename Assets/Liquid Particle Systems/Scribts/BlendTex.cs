using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTex : MonoBehaviour
{

    private Material _material;
    private Renderer _myRenderer;

    private float _blendAmount;
    private float _speed;

    private float _startTime;
    private float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = 0f;
        _speed = 0.01f; // [0.01 : 0.1]
        _myRenderer = GetComponent<Renderer>();

        _material = _myRenderer.material;

        // create new own material
        _myRenderer.sharedMaterial = new Material(_material);

        if (_myRenderer == null)
        {
            Debug.Log("Renderer Component is missing");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentTime += Time.deltaTime;
        _blendAmount = _currentTime * _speed;

        if (_blendAmount < 1)
        {
            _myRenderer.sharedMaterial.SetFloat("_Blend", _blendAmount);
        }
        else
        {
            this.enabled = false;
        }
    }

    public void Restart()
    {
        _startTime = 0f;
    }
}
