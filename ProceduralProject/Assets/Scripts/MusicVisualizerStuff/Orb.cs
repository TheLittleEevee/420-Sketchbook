using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Orb : MonoBehaviour
{
    SimpleVis1 viz;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        viz = SimpleVis1.viz;
        GetComponent<MeshRenderer>().material.SetFloat("_TimeOffset", Random.Range(0, 2 * Mathf.PI));
        body = GetComponent<Rigidbody>();
    }

    public void UpdateAudioData(float value)
    {
        transform.localScale = Vector3.one * (transform.localScale.x + value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vToViz = viz.transform.position - transform.position;
        Vector3 dirToViz = vToViz.normalized;

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, .05f);

        body.AddForce(dirToViz * 100 * Time.deltaTime);
    }
}
