using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGravitation : MonoBehaviour
{
    public MyBoid boidPrefab;
    public static List<MyBoid> boids = new List<MyBoid>();
    public static List<MyBoid> boidDestroyQueue = new List<MyBoid>();

    static float G = 1;
    static float MAX_FORCE = 30;

    public int maxBoids = 10;

    private bool lessBoidsButton = false;
    private bool prevLessBoidsButton = false;
    private bool moreBoidsButton = false;
    private bool prevMoreBoidsButton = false;

    // Start is called before the first frame update
    void Start()
    {
        MyBoid bigBoid = Instantiate(boidPrefab);
        bigBoid.MakeBigBoid();
        boids.Add(bigBoid);

        for (int i = 0; i < (maxBoids - 1); i++)
        {
            boids.Add(Instantiate(boidPrefab));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (MyBoid b in boids)
        {
            Gravitate(b);
        }

        lessBoidsButton = false;
        moreBoidsButton = false;

        if (Input.GetKey(KeyCode.LeftBracket))
        {
            lessBoidsButton = true;
        }
        if (Input.GetKey(KeyCode.RightBracket))
        {
            moreBoidsButton = true;
        }

        if (lessBoidsButton && !prevLessBoidsButton && !moreBoidsButton)
        {
            maxBoids--;
            if (maxBoids <= 1) maxBoids = 1;
        }
        if (moreBoidsButton && !prevMoreBoidsButton && !lessBoidsButton)
        {
            maxBoids++;
        }

        if (boids.Count < maxBoids)
        {
            boids.Add(Instantiate(boidPrefab));
        }
        if (boids.Count > maxBoids)
        {
            int deleteThisIndex = Random.Range(1, boids.Count - 1);
            boids[deleteThisIndex].isDead = true;
        }
    }

    void LateUpdate()
    {
        foreach (MyBoid b in boids)
        {
            if (b.isDead)
            {
                boidDestroyQueue.Add(b);
            }
        }

        foreach (MyBoid b in boidDestroyQueue)
        {
            boids.Remove(b);
            Destroy(b.gameObject);
        }

        boidDestroyQueue.Clear();

        prevMoreBoidsButton = moreBoidsButton;
        prevLessBoidsButton = lessBoidsButton;
    }

    void Gravitate(MyBoid thisBoid)
    {
        foreach (MyBoid b in boids)
        {
            FindGravityForce(thisBoid, b);
            if (thisBoid != b && thisBoid.position.x >= b.position.x - 1 && thisBoid.position.x <= b.position.x + 1 && thisBoid.position.y >= b.position.y - 1 && thisBoid.position.y <= b.position.y + 1 && thisBoid.position.z >= b.position.z - 1 && thisBoid.position.z <= b.position.z + 1)
            {
                thisBoid.velocity *= -1;
                b.velocity *= -1;
            }
        }
        thisBoid.isDone = true;

        //Euler integration
        Vector3 acceleration = thisBoid.force / thisBoid.mass;
        //force *= 0;

        thisBoid.velocity += acceleration * Time.deltaTime;
        thisBoid.position += thisBoid.velocity * Time.deltaTime;

        thisBoid.transform.position = thisBoid.position;
    }

    static void FindGravityForce(MyBoid a, MyBoid b)
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
}
