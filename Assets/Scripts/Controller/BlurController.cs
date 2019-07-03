using System;
using UnityEngine;

//behaviour which should lie on the same gameobject as the main camera
public class BlurController : MonoBehaviour
{
    //material that's applied when doing postprocessing
    [SerializeField] private Material[] postprocessMaterials;
    private Material _postprocessMaterial;

    // sine function parameters
    [SerializeField] [Range(0.001f, 2f)] 
    private float _speed = 0.5f;
    private float _maxBlur;
    private float _totalMaxBlur = 0.05f;
    private float _blurSize;
    private float _oldBlurSize;

    [SerializeField] private bool playerOrEnemy; // ich liebes auf eine Oder-Frage mit JA zu antworten
    // true is Player

    private void Start()
    {

        // ********************************************************

        //TODO                  Who am I ?

        playerOrEnemy = transform.root.name == "Player";

        // ********************************************************


        if (postprocessMaterials.Length < 2)
        {
            Debug.Log("GIB MIR NUR ZWEI MATERIALEN!");
            enabled = false;
        }

        _postprocessMaterial = postprocessMaterials[0];
    }

    void Update()
    {
        // How many beer did you drink/ how many did the other hit
        if (playerOrEnemy)
        {
            _maxBlur = GameManager.instance.GetBluePoints() / (float)GameManager.MaxPoints;
            _maxBlur *= _totalMaxBlur;
        }
        else if (playerOrEnemy)
        {
            _maxBlur = GameManager.instance.GetRedPoints() / (float)GameManager.MaxPoints;
            _maxBlur *= _totalMaxBlur;
        }
        else
        {
            _maxBlur = 0.0f;
        }

        // _maxBlur = 0.05f;


        // calculate sine function 
        _blurSize = _maxBlur * Mathf.Sin(Time.time * _speed);

        // dont change often
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