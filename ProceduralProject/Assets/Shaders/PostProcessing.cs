using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    public Shader shader;
    private Material mat;

    public Texture noiseTexture;

    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(shader);

        mat.SetTexture("_NoiseTex", noiseTexture);
    }

    //Called on a camera when it's rendering a screen
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}
