using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringTarget : MonoBehaviour
{
    public float maxDis = 5;

    private Vector3 target;
    private Vector3 target2;

    private float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            target = new Vector3(Random.Range(-maxDis, maxDis), Random.Range(-maxDis, maxDis), Random.Range(-maxDis, maxDis));
            cooldown = Random.Range(1, 2);
        }

        target2 = Vector3.Lerp(target2, target, .05f);

        transform.position = Vector3.Lerp(transform.position, target2, .03f);
    }
}
