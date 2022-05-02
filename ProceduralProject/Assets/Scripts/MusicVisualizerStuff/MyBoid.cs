using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBoid : MonoBehaviour
{
    public Vector3 position = new Vector3(0, 0, 0);
    public Vector3 force;
    public Vector3 velocity;

    public float size;
    public float mass;
    private bool isBig = false;

    public bool isDone = false;
    public bool isDead = false;

    void Start()
    {
        if (!isBig)
        {
            position = new Vector3(Random.Range(-40, -20), Random.Range(-10, 10), Random.Range(-10, 10));
            mass = Random.Range(5, 20);
            size = mass / 10;
            //size = Mathf.Sqrt(mass);
            transform.localScale = new Vector3(size, size, size);
        }

        transform.position = position;
    }

    void Update()
    {
        
    }

    public void MakeBigBoid()
    {
        isBig = true;
        mass = 50;
        size = mass / 10;
        //size = Mathf.Sqrt(mass);
        transform.localScale = new Vector3(size, size, size);
        position = new Vector3(-30, 5, 0);
    }

    private void LateUpdate() //Runs after all updates are done
    {
        isDone = false;
        force *= 0;
    }

    public void AddForce(Vector3 f)
    {
        force += f;
    }
}
