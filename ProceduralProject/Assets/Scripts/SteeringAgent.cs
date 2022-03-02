using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    private Vector3 force;
    private Vector3 vel;
    private Vector3 target;

    public SteeringTarget steeringTarget;

    [Range(.5f, 10)]
    public float mass = 1;

    [Range(5, 50)]
    public float maxSpeed = 10;

    public float maxForce = 10;
    public float targetAngle = 0;
    public float targetRadius = 100;

    private Vector3 offset;
    private Vector3 offsetRotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        maxSpeed = Random.Range(5, 50);
        mass = Random.Range(.5f, 10);
        maxForce = Random.Range(5, 15);
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetRadius = Random.Range(50, 150);

        offset = Random.onUnitSphere; //Gives random direction vector
        offsetRotationVelocity = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
    }

    // Update is called once per frame
    void Update()
    {
        //To do: Rotate the offset
        Vector3 temp = offsetRotationVelocity * Time.deltaTime;
        offset = Quaternion.Euler(temp) * offset;

        target = steeringTarget.transform.position + offset;

        Steer();
        Euler();

        float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target, p);
    }

    void Euler()
    {
        Vector3 acceleration = force / mass;
        vel += acceleration * Time.deltaTime;
        transform.position += vel * Time.deltaTime;
        force *= 0;
    }

    void Steer()
    {
        Vector3 desiredVel = (target - transform.position).normalized * maxSpeed;

        Vector3 steering = desiredVel - vel;
        steering.Clamp(-maxForce, maxForce);

        force += steering;
    }
}
