using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float upDownAngle = 0;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        speed = Time.deltaTime * 100;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, -1, 0) * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 1, 0) * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(upDownAngle < 88)
            {
                upDownAngle += speed;
                transform.Rotate(new Vector3(1, 0, 0) * speed, Space.Self);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (upDownAngle > -88)
            {
                upDownAngle -= speed;
                transform.Rotate(new Vector3(-1, 0, 0) * speed, Space.Self);
            }
        }
    }
}
