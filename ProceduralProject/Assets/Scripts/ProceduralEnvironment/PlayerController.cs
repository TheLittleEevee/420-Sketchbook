using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody body;

    public GameObject cameraTarget;

    public int gold = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var forward = cameraTarget.transform.forward;
        var right = cameraTarget.transform.right;
        var up = cameraTarget.transform.up;

        if (Input.GetKey(KeyCode.W))
        {
            forward.y = 0;
            body.position += forward.normalized * Time.deltaTime * 30;
            //transform.Translate(forward.normalized * Time.deltaTime * 30);
            //body.AddForce(forward.normalized * Time.deltaTime * 30);
            //body.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 30;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward.y = 0;
            body.position -= forward.normalized * Time.deltaTime * 30;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right.y = 0;
            body.position += right.normalized * Time.deltaTime * 30;
        }
        if (Input.GetKey(KeyCode.A))
        {
            right.y = 0;
            body.position -= right.normalized * Time.deltaTime * 30;
        }
        /*
        if (Input.GetKey(KeyCode.Space))
        {
            body.position += new Vector3(0, 1, 0) * Time.deltaTime * 15;
        }
        */
    }
}
