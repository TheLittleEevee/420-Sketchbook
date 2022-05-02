using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPostProcessing : MonoBehaviour
{
    public Shader shader;
    private Material mat;
    public Texture noiseTexture;

    public float distortion = 1;

    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(shader);
        mat.SetTexture("_NoiseTex", noiseTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Comma))
        {
            distortion -= Time.deltaTime;
            if (distortion < 0) distortion = 0;
        }
        if (Input.GetKey(KeyCode.Period))
        {
            distortion += Time.deltaTime;
            if (distortion > 2.5f) distortion = 2.5f;
        }

        UpdateDistortion(distortion);
    }

    public void UpdateDistortion(float dist)
    {
        mat.SetFloat("_Distort", dist);
    }

    public void UpdateFromAudio(float value)
    {
        mat.SetFloat("_Amp", value);
    }

    //Called on a camera when it's rendering a screen
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}
