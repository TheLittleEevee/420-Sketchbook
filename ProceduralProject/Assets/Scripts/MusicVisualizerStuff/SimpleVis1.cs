using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class SimpleVis1 : MonoBehaviour
{
    public float radius = 500;
    public float amp = 4;

    public float orbHeight = 10;
    public int numBands = 512; //Must be a power of 2
    public Transform prefabOrb;

    private AudioSource player;
    private LineRenderer line;

    private List<Transform> orbs = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();

        //Spawn 1 orb for each frequency band
        Quaternion q = Quaternion.identity;
        for (int i = 0; i < numBands; i++)
        {
            Vector3 p = new Vector3(0, i * orbHeight / numBands, 0);
            orbs.Add(Instantiate(prefabOrb, p, q, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaveForm();
        UpdateFreqBands();
    }

    private void UpdateFreqBands()
    {
        float[] bands = new float[numBands];
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < orbs.Count; i++)
        {
            float p = (i + 1) / (float)numBands;
            orbs[i].localScale = Vector3.one * bands[i] * 200 * p;
            orbs[i].position = new Vector3(0, i * orbHeight / numBands, 0);
        }
    }

    private void UpdateWaveForm()
    {
        int samples = 1024;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);

        Vector3[] points = new Vector3[samples];

        for (int i = 0; i < data.Length; i++)
        {
            float sample = data[i];

            float rads = Mathf.PI * 2 * i / samples;

            float x = Mathf.Cos(rads) * radius;
            float y = sample * amp;
            float z = Mathf.Sin(rads) * radius;

            points[i] = new Vector3(x, y, z);
        }

        line.positionCount = points.Length;
        line.SetPositions(points);
    }
}
