using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorROffset", Random.value);
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorGOffset", Random.value);
        this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorBOffset", Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFromAudio(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0, .1f) * 10;
        int randomChoice = Random.Range(0, 3);
        if (randomChoice == 0) this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorROffset", clampedValue);
        if (randomChoice == 1) this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorBOffset", clampedValue);
        if (randomChoice == 2) this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ColorGOffset", clampedValue);

        transform.localScale = new Vector3(transform.localScale.x, .1f + (2 * value), transform.localScale.z);
    }
}
