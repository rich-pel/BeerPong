using UnityEngine;
// using time

//behaviour which should lie on the same gameobject as the main camera
public class BlurController : MonoBehaviour {
    //material that's applied when doing postprocessing
    [SerializeField]
    private Material postprocessMaterial;

    
    // sine function parameters
    private float amp;

    private float time;
    // sine output parameters

    
    
    void Update()
    {
        // calculate sine function 
        
        // change Material to sine output
        Debug.Log("nix");
    }
    
    
    
    
    // use sine output
    // method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination){
        //draws the pixels from the source texture to the destination texture
        // var temporaryTexture = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, destination, postprocessMaterial, 0);
        // Graphics.Blit(temporaryTexture, destination, postprocessMaterial, 1);
        // RenderTexture.ReleaseTemporary(temporaryTexture);
    }
}