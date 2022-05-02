using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(LineRenderer))]
public class MyVisualizer : MonoBehaviour
{
    private AudioSource musicPlayer;

    private int numSpectrumSamples = 64; //Must be power of 2 between 64 and 8192

    public GridCube cubePrefab;
    private List<GridCube> cubes = new List<GridCube>();
    private List<GridCube> cubes2 = new List<GridCube>();
    GridCube[,] cubes3;

    private LineRenderer line;

    public MyPostProcessing ppShader;

    private bool genButton = false;
    private bool prevGenButton = false;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();

        for (int i = 0; i < Mathf.Sqrt(numSpectrumSamples); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(numSpectrumSamples); j++)
            {
                Vector3 p = new Vector3(i - Mathf.Sqrt(numSpectrumSamples)/2 + .5f, 0, j - Mathf.Sqrt(numSpectrumSamples)/2 + .5f);
                GridCube newCube = Instantiate(cubePrefab, p, Quaternion.identity, transform);
                cubes.Add(newCube);
                cubes2.Add(newCube);
            }
        }
        Reorder();

        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        genButton = false;

        if (Input.GetKey(KeyCode.RightShift))
        {
            genButton = true;
        }

        if (genButton && !prevGenButton)
        {
            Reorder();
        }

        UpdateWave();
        UpdateSpectrum();

        prevGenButton = genButton;
    }

    private void UpdateWave()
    {
        int numSamples = 1024; //Must be power of 2 between 64 and 8192
        float[] audioSamples = new float[numSamples];
        musicPlayer.GetOutputData(audioSamples, 0);

        Vector3[] points = new Vector3[numSamples];

        float avgAmp = 0;

        for (int i = 0; i < audioSamples.Length; i++)
        {
            float sample = audioSamples[i];

            float rads = Mathf.PI * 2 * i / numSamples;

            float y = sample * 5;

            float z = i + (i/numSamples);
            points[i] = new Vector3(-10, y + 5, Mathf.Lerp(-7.5f, 7.5f, Mathf.InverseLerp(0, 1023, z)));

            avgAmp += audioSamples[i]; //Add to average
        }

        avgAmp /= numSamples;
        avgAmp *= 100;

        avgAmp = Mathf.Clamp(Mathf.Abs(avgAmp), 0, 2);

        ppShader.UpdateFromAudio(avgAmp/2);

        line.material.SetFloat("_Speed", avgAmp);
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    private void UpdateSpectrum()
    {
        float[] audioSamples = new float[numSpectrumSamples];
        musicPlayer.GetSpectrumData(audioSamples, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < cubes.Count; i++)
        {
            float p = (i + 1) / (float)numSpectrumSamples; //Takes each frequency band, figures out where it is, then bends its final output (To equalize the visualizer a bit)
            cubes[i].UpdateFromAudio(audioSamples[i] * p * 100);
        }
    }

    public void Reorder()
    {
        for (int i = 0; i < cubes2.Count; i++)
        {
            GridCube temp = cubes2[i];
            int randomIndex = Random.Range(i, cubes2.Count);
            cubes2[i] = cubes2[randomIndex];
            cubes2[randomIndex] = temp;
        }

        for (int i = 0; i < cubes2.Count; i++)
        {
            float newZ = 0;
            float newX = 0;
            
            if (i % 8 == 1) newZ = -3.5f;
            if (i % 8 == 2) newZ = -2.5f;
            if (i % 8 == 3) newZ = -1.5f;
            if (i % 8 == 4) newZ = -0.5f;
            if (i % 8 == 5) newZ = 0.5f;
            if (i % 8 == 6) newZ = 1.5f;
            if (i % 8 == 7) newZ = 2.5f;
            if (i % 8 == 0) newZ = 3.5f;

            if (Mathf.Floor(i / 8) == 0) newX = -3.5f;
            if (Mathf.Floor(i / 8) == 1) newX = -2.5f;
            if (Mathf.Floor(i / 8) == 2) newX = -1.5f;
            if (Mathf.Floor(i / 8) == 3) newX = -0.5f;
            if (Mathf.Floor(i / 8) == 4) newX = 0.5f;
            if (Mathf.Floor(i / 8) == 5) newX = 1.5f;
            if (Mathf.Floor(i / 8) == 6) newX = 2.5f;
            if (Mathf.Floor(i / 8) == 7) newX = 3.5f;
            
            Vector3 p = transform.position;
            p.x = newX;
            p.z = newZ;
            cubes2[i].gameObject.transform.position = p;
        }




        cubes3 = new GridCube[(int)Mathf.Sqrt(numSpectrumSamples), (int)Mathf.Sqrt(numSpectrumSamples)];

        int x = 0;
        int y = 0;

        for (int i = 0; i < cubes2.Count; i++)
        {
            if (i % 8 == 1) y = 0;
            if (i % 8 == 2) y = 1;
            if (i % 8 == 3) y = 2;
            if (i % 8 == 4) y = 3;
            if (i % 8 == 5) y = 4;
            if (i % 8 == 6) y = 5;
            if (i % 8 == 7) y = 6;
            if (i % 8 == 0) y = 7;

            if (Mathf.Floor(i / 8) == 0) x = 0;
            if (Mathf.Floor(i / 8) == 1) x = 1;
            if (Mathf.Floor(i / 8) == 2) x = 2;
            if (Mathf.Floor(i / 8) == 3) x = 3;
            if (Mathf.Floor(i / 8) == 4) x = 4;
            if (Mathf.Floor(i / 8) == 5) x = 5;
            if (Mathf.Floor(i / 8) == 6) x = 6;
            if (Mathf.Floor(i / 8) == 7) x = 7;

            cubes3[x, y] = cubes2[i];
            cubes3[x, y].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_AlphaOffset", 0);
        }

        walkRooms();
        walkRooms();
        walkRooms();

        punchHoles();
    }

    private void walkRooms()
    {
        int halfW = (int)Mathf.Sqrt(numSpectrumSamples) / 2;
        int halfH = (int)Mathf.Sqrt(numSpectrumSamples) / 2;

        int sx = (int)Random.Range(0, Mathf.Sqrt(numSpectrumSamples));
        int sy = (int)Random.Range(0, Mathf.Sqrt(numSpectrumSamples));
        int tx = (int)Random.Range(0, halfW);
        int ty = (int)Random.Range(0, halfH);

        if (sx < halfW) tx += halfW; //If starting point on left, move end point to right
        if (sy < halfH) ty += halfH; //Move end to bottom half of dungeon

        cubes3[sx, sy].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_AlphaOffset", 1);
        cubes3[tx, ty].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_AlphaOffset", 1);

        int n = 0;

        while (sx != tx || sy != ty)
        {
            int dir = (int)Random.Range(0, 4); //0 to 3
            int dis = (int)Random.Range(1, 4); //1 to 3

            n++;

            if (Random.Range(0, 100) > 50)
            {
                int dx = tx - sx;
                int dy = ty - sy;

                if (Mathf.Abs(dx) < Mathf.Abs(dy))
                { // We are closer on x-axis than y
                    dir = (dy < 0) ? 0 : 1;
                }
                else
                { // We are closer on y-axis than x
                    dir = (dx < 0) ? 3 : 2;
                }
            }



            for (int i = 0; i < dis; i++)
            {
                switch (dir)
                {
                    case 0:
                        sy--; //Move north
                        break;
                    case 1:
                        sy++; //Move south
                        break;
                    case 2:
                        sx++; //Move east
                        break;
                    case 3: //Move west
                        sx--;
                        break;
                }
                if (sx < 0) sx = 0;
                if (sx > (int)Mathf.Sqrt(numSpectrumSamples) - 1) sx = (int)Mathf.Sqrt(numSpectrumSamples) - 1;
                if (sy < 0) sy = 0;
                if (sy > (int)Mathf.Sqrt(numSpectrumSamples) - 1) sy = (int)Mathf.Sqrt(numSpectrumSamples) - 1;

                cubes3[sx, sy].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_AlphaOffset", 1);

            } //End for
        } //End while
    }

    private void punchHoles()
    {
        for (int x = 0; x < cubes3.GetLength(0); x++)
        {
            for (int y = 0; y < cubes3.GetLength(1); y++)
            {
                if (cubes3[x, y].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetFloat("_AlphaOffset") == 0) continue; //Only consider visible cubes

                if (Random.Range(0, 100) < 25) continue; //25% of time, don't punch holes

                GridCube[] neighbors = new GridCube[8];

                if (y-1 >= 0) neighbors[0] = cubes3[x, y - 1]; //Above
                else neighbors[0] = null;
                if (x+1 < cubes3.GetLength(0) && y-1 >= 0) neighbors[1] = cubes3[x + 1, y - 1];
                else neighbors[1] = null;
                if (x+1 < cubes3.GetLength(0)) neighbors[2] = cubes3[x + 1, y]; //Right
                else neighbors[2] = null;
                if (x+1 < cubes3.GetLength(0) && y+1 < cubes3.GetLength(1)) neighbors[3] = cubes3[x + 1, y + 1];
                else neighbors[3] = null;
                if (y+1 < cubes3.GetLength(1)) neighbors[4] = cubes3[x, y + 1]; //Below
                else neighbors[4] = null;
                if (x-1 >= 0 && y+1 < cubes3.GetLength(1)) neighbors[5] = cubes3[x - 1, y + 1];
                else neighbors[5] = null;
                if (x-1 >= 0) neighbors[6] = cubes3[x - 1, y]; //Left
                else neighbors[6] = null;
                if (x-1 >= 0 && y-1 >= 0) neighbors[7] = cubes3[x - 1, y - 1];
                else neighbors[7] = null;

                bool isSolid = (neighbors[7] == null) ? false : neighbors[7].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetFloat("_AlphaOffset") == 1;
                int tally = 0;

                foreach (GridCube n in neighbors)
                {
                    bool s = (n == null) ? false : n.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.GetFloat("_AlphaOffset") == 1;

                    if (s != isSolid) tally++;

                    isSolid = s;
                }
                if (tally <= 2)
                {
                    //Safe to punch a hole
                    cubes3[x, y].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_AlphaOffset", 0);
                }
            }
        }
    }
}
