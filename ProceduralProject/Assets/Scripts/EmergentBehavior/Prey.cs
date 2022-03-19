using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
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
    public float foodBeatsFearAmount;

    public Vector3 target;
    public float targetDis = 1000000000000000000;

    public float maxHideCounter;
    public float curHideCounter = 0;

    public float maxBreedCounter = 10;
    public float curBreedCounter = 10;
    public bool canBreed = false;

    public bool isDead = false;
    public bool isHidden = false;
    public bool goHide = false;
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
            mass = Random.Range(.5f, 2);
            //scaleModifier = Random.Range(1, 2);
            scaleModifier = (((mass - .5f) * (2 - 1)) / (2 - .5f)) + 1; //Like Processing's Map function
            transform.localScale = new Vector3(1, 1, 1) * scaleModifier;
            maxSpeed = Random.Range(16, 25);
            maxHideCounter = Random.Range(5, 20);
            breedOn = Random.Range(90, 110);
            breedOff = Random.Range(60, 80);
            foodBeatsFearAmount = Random.Range(15, 25);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hunger -= Time.deltaTime * .5f;
        if (hunger <= 0)
        {
            isDead = true;
            //print("Dead prey");
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

        if (isHidden)
        {
            curHideCounter -= Time.deltaTime;
            if (curHideCounter <= 0 || hunger < foodBeatsFearAmount)
            {
                isHidden = false;
                goHide = false;
            }
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
            transform.position = new Vector3 (transform.position.x, transform.position.y, -200);
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

    public void BeHidden()
    {
        isHidden = true;
        velocity *= 0;
        curHideCounter = maxHideCounter;
    }

    public void Newborn(GameObject a, GameObject b)
    {
        isBaby = true;
        hunger = 80;
        goBreed = false;
        mass = Random.Range(a.GetComponent<Prey>().mass, b.GetComponent<Prey>().mass);
        transform.position = new Vector3(Random.Range(a.GetComponent<Prey>().transform.position.x, b.GetComponent<Prey>().transform.position.x), .5f, Random.Range(a.GetComponent<Prey>().transform.position.z, b.GetComponent<Prey>().transform.position.z));
        scaleModifier = (((mass - .5f) * (2 - 1)) / (2 - .5f)) + 1; //Like Processing's Map function
        transform.localScale = new Vector3(1, 1, 1) * scaleModifier;
        maxSpeed = Random.Range(a.GetComponent<Prey>().maxSpeed, b.GetComponent<Prey>().maxSpeed);
        maxHideCounter = Random.Range(a.GetComponent<Prey>().maxHideCounter, b.GetComponent<Prey>().maxHideCounter);
        breedOn = Random.Range(a.GetComponent<Prey>().breedOn, b.GetComponent<Prey>().breedOn);
        breedOff = Random.Range(a.GetComponent<Prey>().breedOff, b.GetComponent<Prey>().breedOff);
        foodBeatsFearAmount = Random.Range(a.GetComponent<Prey>().foodBeatsFearAmount, b.GetComponent<Prey>().foodBeatsFearAmount);
    }
}
