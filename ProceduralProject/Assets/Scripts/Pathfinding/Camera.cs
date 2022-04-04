using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private float upDownAngle = -60;
    private float speed = 10;
    private float rotSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var forward = transform.forward;
        var right = transform.right;
        var up = transform.up;

        if (Input.GetKey(KeyCode.A) && Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotSpeed, Space.World);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            right.y = 0;
            transform.position -= right.normalized * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D) && Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            right.y = 0;
            transform.position += right.normalized * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetMouseButton(1))
        {
            if (upDownAngle < 0)
            {
                upDownAngle += rotSpeed * Time.deltaTime;
                transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime * rotSpeed, Space.Self);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(1))
        {
            if (upDownAngle > -88)
            {
                upDownAngle -= rotSpeed * Time.deltaTime;
                transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * rotSpeed, Space.Self);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime * speed;
        }


        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * speed / 2;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * speed / 2;
        }
    }
}
