using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;

    public GameObject cameraTarget;

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
            //body.AddForce(forward);

            forward.y = 0;
            body.position += forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //body.AddForce(-forward);

            forward.y = 0;
            body.position -= forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //body.AddForce(right);

            right.y = 0;
            body.position += right * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //body.AddForce(-right);

            right.y = 0;
            body.position -= right * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //body.AddForce(up);

            //body.position += up * Time.deltaTime * 10;

            body.position += new Vector3(0, 1, 0) * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //body.AddForce(-up);

            //body.position -= up * Time.deltaTime * 20;
        }
    }
}
