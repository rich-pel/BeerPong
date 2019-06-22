﻿using System;
using UnityEngine;

//behaviour which should lie on the same gameobject as the main camera
public class BlurController : MonoBehaviour
{
    //material that's applied when doing postprocessing
    [SerializeField]
    private Material[] postprocessMaterials;
    private Material _postprocessMaterial;

    // sine function parameters
    [SerializeField] [Range(0.001f, 2f)] 
    private float _speed = 1f;
    private float _maxBlur;
    private float _blurSize;
    private float _oldBlurSize;

    private void Start()
    {
        if(postprocessMaterials.Length < 2 )
            Debug.Log("GIB MIR NUR ZWEI MATERIALEN!");
        _speed = 1f;
        _postprocessMaterial = postprocessMaterials[0];
    }

    void Update()
    {
        // How many Bier did you drink, but do not divide by zero
        if (GameManager.instance.GetMaxPoints() != 0)
            _maxBlur = (float) (GameManager.instance.GetEnemyPoints() / GameManager.instance.GetMaxPoints());
        else
            _maxBlur = 0.1f;
        
        // calculate sine function 
        _blurSize = _maxBlur * Mathf.Sin(Time.time * _speed);

        if (_oldBlurSize < 0 && 0 <= _blurSize)
            _postprocessMaterial = postprocessMaterials[0];
        else if (_blurSize < 0 && 0 <= _oldBlurSize)
            _postprocessMaterial = postprocessMaterials[1];
        
        // change Material to sine output
        _postprocessMaterial.SetFloat("_BlurSize", _blurSize);

        _oldBlurSize = _blurSize;
    }

    // method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        //draws the pixels from the source texture to the destination texture
        Graphics.Blit(source, destination, _postprocessMaterial);
    }
}