using System;
using UnityEngine;
// using time

//behaviour which should lie on the same gameobject as the main camera
public class BlurController : MonoBehaviour {
    //material that's applied when doing postprocessing
    [SerializeField]
    private Material postprocessMaterial;

    [SerializeField] 
    private GameManager gm;
    
    // sine function parameters
    //[SerializeField] [Range(0.0001f, 0.1f)] 
    public float speed = 0.1f;
    public float maxBlur = 0.5f;
    public float blurSize;
    
    void Update()
    {        
        // TODO: Becher getrunken /10 
        // blurSize = gm.gegner.punkte / 10;
        // GameManager.instance.GetEnemyPoints() / GameManager.instance.GetMaxPoints();
        
        
        // calculate sine function 
        blurSize = maxBlur * Mathf.Sin(Time.time * speed);

        Debug.Log("blurSize: " + blurSize);
        

        // change Material to sine output
        postprocessMaterial.SetFloat("_BlurSize", Math.Abs(blurSize));
        // Blur direction
        // postprocessMaterial.SetPass(blurSize < 0 ? 0 : 1);

        // var debuf = postprocessMaterial.GetFloat("_BlurSize");
        // Debug.Log("sine: " + y +  " mat:" + debuf);
        
    }
    
    
    
    
    // use sine output
    // method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        //draws the pixels from the source texture to the destination texture
        Graphics.Blit(source, destination, postprocessMaterial, 0);
    }
}