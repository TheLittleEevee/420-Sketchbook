using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float upDownAngle = 0;
    private float speed;

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

        if (Input.GetKey(KeyCode.W))
        {
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.D))
        {
            right.y = 0;
            transform.position += right.normalized * Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.A))
        {
            right.y = 0;
            transform.position -= right.normalized * Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * 15;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * 15;
        }

        speed = Time.deltaTime * 100;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -1, 0) * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 1, 0) * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (upDownAngle < 88)
            {
                upDownAngle += speed;
                transform.Rotate(new Vector3(-1, 0, 0) * speed, Space.Self);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (upDownAngle > -88)
            {
                upDownAngle -= speed;
                transform.Rotate(new Vector3(1, 0, 0) * speed, Space.Self);
            }
        }
    }
}
