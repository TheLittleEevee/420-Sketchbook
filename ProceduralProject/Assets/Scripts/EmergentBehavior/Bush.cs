using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public float lifespan;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-175, 175), .5f, Random.Range(-175, 175));
        lifespan = Random.Range(40, 100);
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;

        if (lifespan <= 0)
        {
            isDead = true;
        }
    }
}
