using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public float hungerValue;
    public float scaleModifier;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        hungerValue = Random.Range(5, 15);
        transform.position = new Vector3(Random.Range(-175, 175), .5f, Random.Range(-175, 175));
        //scaleModifier = Random.Range(.1f, .3f);
        scaleModifier = (((hungerValue - 5) * (.3f - .1f)) / (15 - 5)) + .1f; //Like Processing's Map function
        transform.localScale = new Vector3(1, 1, 1) * scaleModifier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
