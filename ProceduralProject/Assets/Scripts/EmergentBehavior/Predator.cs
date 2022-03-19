using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 force;
    public float mass;
    public float scaleModifier;
    public float maxSpeed = 10;
    public float maxForce = 10;
    public float hunger = 80;
    public float breedOn;
    public float breedOff;

    public Vector3 target;
    public float targetDis = 1000000000000000000;

    public float maxBreedCounter = 10;
    public float curBreedCounter = 10;
    public bool canBreed = false;

    public bool isDead = false;
    public bool goBreed = false;
    bool isBaby = false;

    // Start is called before the first frame update
    void Start()
    {
        hunger = 80;
        goBreed = false;
        if (!isBaby)
        {
            transform.position = new Vector3(Random.Range(-175, 175), .5f, Random.Range(-175, 175));
            target = transform.position;
            mass = Random.Range(2, 5);
            //scaleModifier = Random.Range(2, 5);
            scaleModifier = (((mass - 2) * (5 - 2)) / (5 - 2)) + 2; //Like Processing's Map function
            transform.localScale = new Vector3(1, 1, 1) * scaleModifier;
            maxSpeed = Random.Range(15, 20);
            breedOn = Random.Range(80, 100);
            breedOff = Random.Range(50, 70);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= Time.deltaTime * .25f;
        if (hunger <= 0)
        {
            isDead = true;
            //print("Dead predator");
        }

        if (curBreedCounter > 0)
        {
            curBreedCounter -= Time.deltaTime;
            if (curBreedCounter <= 0) canBreed = true;
        }

        if (hunger > 150)
        {
            hunger = 150;
        }
        if (hunger >= breedOn)
        {
            goBreed = true;
        }
        if (hunger <= breedOff)
        {
            goBreed = false;
        }

        Steer();
        Euler();
    }

    void Euler()
    {
        Vector3 acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;
        if (transform.position.x > 200)
        {
            velocity *= 0;
            acceleration *= 0;
            transform.position = new Vector3(200, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -200)
        {
            velocity *= 0;
            acceleration *= 0;
            transform.position = new Vector3(-200, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > 200)
        {
            velocity *= 0;
            acceleration *= 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, 200);
        }
        else if (transform.position.z < -200)
        {
            velocity *= 0;
            acceleration *= 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, -200);
        }
        transform.position += velocity * Time.deltaTime;
        force *= 0;
    }

    void Steer()
    {
        Vector3 desiredVel = (target - transform.position).normalized * maxSpeed;

        Vector3 steering = desiredVel - velocity;
        steering.Clamp(-maxForce, maxForce);

        force += steering;
    }

    public void Newborn(GameObject a, GameObject b)
    {
        isBaby = true;
        hunger = 80;
        goBreed = false;
        mass = Random.Range(a.GetComponent<Predator>().mass, b.GetComponent<Predator>().mass);
        transform.position = new Vector3(Random.Range(a.GetComponent<Predator>().transform.position.x, b.GetComponent<Predator>().transform.position.x), .5f, Random.Range(a.GetComponent<Predator>().transform.position.z, b.GetComponent<Predator>().transform.position.z));
        scaleModifier = (((mass - 2) * (5 - 2)) / (5 - 2)) + 2; //Like Processing's Map function
        transform.localScale = new Vector3(1, 1, 1) * scaleModifier;
        maxSpeed = Random.Range(a.GetComponent<Predator>().maxSpeed, b.GetComponent<Predator>().maxSpeed);
        breedOn = Random.Range(a.GetComponent<Predator>().breedOn, b.GetComponent<Predator>().breedOn);
        breedOff = Random.Range(a.GetComponent<Predator>().breedOff, b.GetComponent<Predator>().breedOff);
    }
}
