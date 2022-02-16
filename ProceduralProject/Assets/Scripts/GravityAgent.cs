using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAgent : MonoBehaviour
{
    static float G = 1;
    static float MAX_FORCE = 10;
    static List<GravityAgent> agents = new List<GravityAgent>();
    
    static void FindGravityForce(GravityAgent a, GravityAgent b)
    {
        if (a == b) return;
        if (a.isDone) return;
        if (b.isDone) return;

        Vector3 vectorToB = b.position - a.position;
        float gravity = G * (a.mass * b.mass) / vectorToB.sqrMagnitude;

        if (gravity > MAX_FORCE) gravity = MAX_FORCE;

        vectorToB.Normalize();

        a.AddForce(vectorToB * gravity);
        b.AddForce(-vectorToB * gravity);
    }

    Vector3 position;
    Vector3 force;
    Vector3 velocity;

    float mass;
    bool isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        mass = Random.Range(10, 100);

        agents.Add(this);
    }

    private void OnDestroy()
    {
        agents.Remove(this);
    }

    public void AddForce(Vector3 f)
    {
        force += f;
    }

    // Update is called once per frame
    void Update()
    {
        //Calc gravity to every other agent
        foreach(GravityAgent a in agents)
        {
            FindGravityForce(this, a);
        }
        isDone = true;

        //Euler integration
        Vector3 acceleration = force / mass;
        //force *= 0;

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        transform.position = position;
    }

    private void LateUpdate() //Runs after all updates are done
    {
        isDone = false;
        force *= 0;
    }
}
