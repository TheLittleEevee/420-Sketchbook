using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergentBehavior : MonoBehaviour
{
    public GameObject predator;
    public GameObject prey;
    public GameObject grass;
    public GameObject bush;

    public static List<GameObject> allPredators = new List<GameObject>();
    public static List<GameObject> allPrey = new List<GameObject>();
    public static List<GameObject> allGrass = new List<GameObject>();
    public static List<GameObject> allBushes = new List<GameObject>();

    public static List<GameObject> preyBabyQueue = new List<GameObject>();
    public static List<GameObject> predatorBabyQueue = new List<GameObject>();

    bool spawnPreyButton = false;
    bool prevSpawnPreyButton = false;
    bool spawnPredButton = false;
    bool prevSpawnPredButton = false;

    // Start is called before the first frame update
    void Start()
    {
        allPredators.Add(Instantiate(predator));
        allPredators.Add(Instantiate(predator));
        allPredators.Add(Instantiate(predator));

        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
        allPrey.Add(Instantiate(prey));
    }

    // Update is called once per frame
    void Update()
    {
        //Record Button Presses
        spawnPredButton = false;
        spawnPreyButton = false;
        if (Input.GetKey(KeyCode.RightShift))
        {
            spawnPreyButton = true;
        }
        if (Input.GetKey(KeyCode.Return))
        {
            spawnPredButton = true;
        }

        //Spawn new objects
        if (allGrass.Count < 100)
        {
            allGrass.Add(Instantiate(grass));
        }
        if (allBushes.Count < 10)
        {
            allBushes.Add(Instantiate(bush));
        }
        if (spawnPreyButton && !prevSpawnPreyButton)
        {
            allPrey.Add(Instantiate(prey));
        }
        if (spawnPredButton && !prevSpawnPredButton)
        {
            allPredators.Add(Instantiate(predator));
        }

        //Targeting
        foreach (GameObject p in allPredators)
        {
            GetTarget(p, "Predator");
        }

        foreach (GameObject p in allPrey)
        {
            GetTarget(p, "Prey");
        }
    }

    void LateUpdate()
    {
        foreach (GameObject baby in preyBabyQueue)
        {
            allPrey.Add(baby);
        }
        foreach (GameObject baby in predatorBabyQueue)
        {
            allPredators.Add(baby);
        }
        preyBabyQueue.Clear();
        predatorBabyQueue.Clear();

        foreach (GameObject p in allPredators)
        {
            p.GetComponent<Predator>().targetDis = 1000000000000000000;
            p.GetComponent<Predator>().target = new Vector3 (p.transform.position.x, .5f, p.transform.position.z);
            if (p.GetComponent<Predator>().isDead)
            {
                allPredators.Remove(p);
                Destroy(p);
                break;
            }
        }
        foreach (GameObject p in allPrey)
        {
            p.GetComponent<Prey>().targetDis = 1000000000000000000;
            p.GetComponent<Prey>().goHide = false;
            if (p.GetComponent<Prey>().isDead)
            {
                allPrey.Remove(p);
                Destroy(p);
                break;
            }
        }
        foreach (GameObject g in allGrass)
        {
            if (g.GetComponent<Grass>().isDead)
            {
                allGrass.Remove(g);
                Destroy(g);
                break;
            }
        }
        foreach (GameObject b in allBushes)
        {
            if (b.GetComponent<Bush>().isDead)
            {
                allBushes.Remove(b);
                Destroy(b);
                break;
            }
        }

        prevSpawnPreyButton = spawnPreyButton;
        prevSpawnPredButton = spawnPredButton;
    }

    void GetTarget(GameObject targeter, string type)
    {
        if (type == "Predator")
        {
            if (targeter.GetComponent<Predator>().goBreed)
            {
                foreach (GameObject p in allPredators)
                {
                    if (p.GetComponent<Predator>().goBreed && p != targeter)
                    {
                        float dx = p.transform.position.x - targeter.transform.position.x;
                        float dz = p.transform.position.z - targeter.transform.position.z;
                        float dis = Mathf.Sqrt(dx * dx + dz * dz);

                        if (targeter.GetComponent<Predator>().target == new Vector3(p.transform.position.x, .5f, p.transform.position.z))
                        {
                            targeter.GetComponent<Predator>().targetDis = dis;
                        }

                        if (dis < targeter.GetComponent<Predator>().targetDis)
                        {
                            targeter.GetComponent<Predator>().targetDis = dis;
                            targeter.GetComponent<Predator>().target = new Vector3(p.transform.position.x, .5f, p.transform.position.z);
                        }

                        if (dis < 3)
                        {
                            targeter.GetComponent<Predator>().hunger /= 1.5f;
                            if (targeter.GetComponent<Predator>().hunger >= targeter.GetComponent<Predator>().breedOn) targeter.GetComponent<Predator>().hunger = targeter.GetComponent<Predator>().breedOn - 1;
                            targeter.GetComponent<Predator>().goBreed = false;
                            p.GetComponent<Predator>().hunger /= 1.5f;
                            if (p.GetComponent<Predator>().hunger >= p.GetComponent<Predator>().breedOn) p.GetComponent<Predator>().hunger = p.GetComponent<Predator>().breedOn - 1;
                            p.GetComponent<Predator>().goBreed = false;

                            GameObject baby = Instantiate(predator);

                            baby.GetComponent<Predator>().Newborn(targeter, p);
                            predatorBabyQueue.Add(baby);
                        }
                    }
                }
            }

            if (targeter.GetComponent<Predator>().targetDis >= 100000000)
            {
                foreach (GameObject p in allPrey)
                {
                    float dx = p.transform.position.x - targeter.transform.position.x;
                    float dz = p.transform.position.z - targeter.transform.position.z;
                    float dis = Mathf.Sqrt(dx * dx + dz * dz);

                    if (dis < 40 && p.GetComponent<Prey>().hunger > p.GetComponent<Prey>().foodBeatsFearAmount)
                    {
                        p.GetComponent<Prey>().goHide = true;
                    }

                    if (p.GetComponent<Prey>().isHidden) continue;

                    if (targeter.GetComponent<Predator>().target == new Vector3(p.transform.position.x, .5f, p.transform.position.z))
                    {
                        targeter.GetComponent<Predator>().targetDis = dis;
                    }

                    if (dis < targeter.GetComponent<Predator>().targetDis)
                    {
                        targeter.GetComponent<Predator>().targetDis = dis;
                        targeter.GetComponent<Predator>().target = new Vector3(p.transform.position.x, .5f, p.transform.position.z);
                    }

                    if (dis < 3)
                    {
                        p.GetComponent<Prey>().isDead = true;
                        targeter.GetComponent<Predator>().targetDis = 1000000000000000000;
                        targeter.GetComponent<Predator>().hunger += p.GetComponent<Prey>().mass;
                    }
                }
            }
        }

        if (type == "Prey")
        {
            if (targeter.GetComponent<Prey>().goHide || targeter.GetComponent<Prey>().isHidden)
            {
                foreach (GameObject b in allBushes)
                {
                    float ddx = b.transform.position.x - targeter.transform.position.x;
                    float ddz = b.transform.position.z - targeter.transform.position.z;
                    float hideDis = Mathf.Sqrt(ddx * ddx + ddz * ddz);

                    if (targeter.GetComponent<Prey>().target == new Vector3(b.transform.position.x, .5f, b.transform.position.z))
                    {
                        targeter.GetComponent<Prey>().targetDis = hideDis;
                    }

                    if (hideDis < targeter.GetComponent<Prey>().targetDis)
                    {
                        targeter.GetComponent<Prey>().targetDis = hideDis;
                        targeter.GetComponent<Prey>().target = new Vector3(b.transform.position.x, .5f, b.transform.position.z);
                    }
                }
                if (targeter.GetComponent<Prey>().targetDis < 1)
                {
                    if (!targeter.GetComponent<Prey>().isHidden) targeter.GetComponent<Prey>().BeHidden();
                }
                else if (targeter.GetComponent<Prey>().targetDis > 3)
                {
                    targeter.GetComponent<Prey>().isHidden = false;
                }
            }
            else if (targeter.GetComponent<Prey>().goBreed)
            {
                foreach (GameObject p in allPrey)
                {
                    if (p.GetComponent<Prey>().goBreed && p != targeter)
                    {
                        float dx = p.transform.position.x - targeter.transform.position.x;
                        float dz = p.transform.position.z - targeter.transform.position.z;
                        float dis = Mathf.Sqrt(dx * dx + dz * dz);

                        if (targeter.GetComponent<Prey>().target == new Vector3(p.transform.position.x, .5f, p.transform.position.z))
                        {
                            targeter.GetComponent<Prey>().targetDis = dis;
                        }

                        if (dis < targeter.GetComponent<Prey>().targetDis)
                        {
                            targeter.GetComponent<Prey>().targetDis = dis;
                            targeter.GetComponent<Prey>().target = new Vector3(p.transform.position.x, .5f, p.transform.position.z);
                        }

                        if (dis < 3)
                        {
                            targeter.GetComponent<Prey>().hunger /= 1.5f;
                            if (targeter.GetComponent<Prey>().hunger >= targeter.GetComponent<Prey>().breedOn) targeter.GetComponent<Prey>().hunger = targeter.GetComponent<Prey>().breedOn - 1;
                            targeter.GetComponent<Prey>().goBreed = false;
                            p.GetComponent<Prey>().hunger /= 1.5f;
                            if (p.GetComponent<Prey>().hunger >= p.GetComponent<Prey>().breedOn) p.GetComponent<Prey>().hunger = p.GetComponent<Prey>().breedOn - 1;
                            p.GetComponent<Prey>().goBreed = false;

                            GameObject baby = Instantiate(prey);

                            baby.GetComponent<Prey>().Newborn(targeter, p);
                            preyBabyQueue.Add(baby);
                        }
                    }
                }
            }
            
            if (targeter.GetComponent<Prey>().targetDis >= 100000000)
            {
                foreach (GameObject g in allGrass)
                {
                    float dx = g.transform.position.x - targeter.transform.position.x;
                    float dz = g.transform.position.z - targeter.transform.position.z;
                    float dis = Mathf.Sqrt(dx * dx + dz * dz);

                    if (targeter.GetComponent<Prey>().target == new Vector3(g.transform.position.x, .5f, g.transform.position.z))
                    {
                        targeter.GetComponent<Prey>().targetDis = dis;
                    }

                    if (dis < targeter.GetComponent<Prey>().targetDis)
                    {
                        targeter.GetComponent<Prey>().targetDis = dis;
                        targeter.GetComponent<Prey>().target = new Vector3(g.transform.position.x, .5f, g.transform.position.z);
                    }

                    if (dis < 3)
                    {
                        g.GetComponent<Grass>().isDead = true;
                        targeter.GetComponent<Prey>().targetDis = 1000000000000000000;
                        targeter.GetComponent<Prey>().hunger += g.GetComponent<Grass>().hungerValue;
                    }
                }
            }
        }
    }
}
