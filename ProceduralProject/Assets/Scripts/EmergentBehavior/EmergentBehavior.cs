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


    public static List<GameObject> predatorDestroyQueue = new List<GameObject>();
    public static List<GameObject> preyDestroyQueue = new List<GameObject>();
    public static List<GameObject> grassDestroyQueue = new List<GameObject>();
    public static List<GameObject> bushDestroyQueue = new List<GameObject>();

    int maxGrass = 100;
    int maxBushes = 10;

    bool spawnPreyButton = false;
    bool prevSpawnPreyButton = false;
    bool spawnPredButton = false;
    bool prevSpawnPredButton = false;
    bool moreGrassButton = false;
    bool prevMoreGrassButton = false;
    bool moreBushesButton = false;
    bool prevMoreBushesButton = false;
    bool lessGrassButton = false;
    bool prevLessGrassButton = false;
    bool lessBushesButton = false;
    bool prevLessBushesButton = false;

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
        moreGrassButton = false;
        moreBushesButton = false;
        lessGrassButton = false;
        lessBushesButton = false;
        if (Input.GetKey(KeyCode.RightShift))
        {
            spawnPreyButton = true;
        }
        if (Input.GetKey(KeyCode.Return))
        {
            spawnPredButton = true;
        }
        if (Input.GetKey(KeyCode.Comma))
        {
            lessGrassButton = true;
        }
        if (Input.GetKey(KeyCode.Period))
        {
            moreGrassButton = true;
        }
        if (Input.GetKey(KeyCode.LeftBracket))
        {
            lessBushesButton = true;
        }
        if (Input.GetKey(KeyCode.RightBracket))
        {
            moreBushesButton = true;
        }

        //Change the max values of Grass and Bushes
        if (lessGrassButton && !moreGrassButton)
        {
            maxGrass--;
            if (maxGrass <= 0) maxGrass = 0;
        }
        if (moreGrassButton && !lessGrassButton)
        {
            maxGrass++;
        }
        if (lessBushesButton && !prevLessBushesButton && !moreBushesButton)
        {
            maxBushes--;
            if (maxBushes <= 0) maxBushes = 0;
        }
        if (moreBushesButton && !prevMoreBushesButton && !lessBushesButton)
        {
            maxBushes++;
        }

        //Spawn new objects
        if (allGrass.Count < maxGrass)
        {
            allGrass.Add(Instantiate(grass));
        }
        if (allGrass.Count > maxGrass)
        {
            int deleteThisIndex = Random.Range(0, allGrass.Count - 1);
            allGrass[deleteThisIndex].GetComponent<Grass>().isDead = true;
            //allGrass.RemoveAt(Random.Range(0, allGrass.Count - 1));
        }
        if (allBushes.Count < maxBushes)
        {
            allBushes.Add(Instantiate(bush));
        }
        if (allBushes.Count > maxBushes)
        {
            int deleteThisIndex = Random.Range(0, allBushes.Count - 1);
            allBushes[deleteThisIndex].GetComponent<Bush>().isDead = true;
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
                predatorDestroyQueue.Add(p);
            }
        }
        foreach (GameObject p in allPrey)
        {
            p.GetComponent<Prey>().targetDis = 1000000000000000000;
            p.GetComponent<Prey>().target = new Vector3(p.transform.position.x, .5f, p.transform.position.z);
            p.GetComponent<Prey>().goHide = false;
            if (p.GetComponent<Prey>().isDead)
            {
                preyDestroyQueue.Add(p);
            }
        }
        foreach (GameObject g in allGrass)
        {
            if (g.GetComponent<Grass>().isDead)
            {
                grassDestroyQueue.Add(g);
            }
        }
        foreach (GameObject b in allBushes)
        {
            if (b.GetComponent<Bush>().isDead)
            {
                bushDestroyQueue.Add(b);
            }
        }



        foreach (GameObject p in predatorDestroyQueue)
        {
            allPredators.Remove(p);
            Destroy(p);
        }
        foreach (GameObject p in preyDestroyQueue)
        {
            allPrey.Remove(p);
            Destroy(p);
        }
        foreach (GameObject g in grassDestroyQueue)
        {
            allGrass.Remove(g);
            Destroy(g);
        }
        foreach (GameObject b in bushDestroyQueue)
        {
            allBushes.Remove(b);
            Destroy(b);
        }
        predatorDestroyQueue.Clear();
        preyDestroyQueue.Clear();
        grassDestroyQueue.Clear();
        bushDestroyQueue.Clear();

        prevSpawnPreyButton = spawnPreyButton;
        prevSpawnPredButton = spawnPredButton;
        prevMoreGrassButton = moreGrassButton;
        prevMoreBushesButton = moreBushesButton;
        prevLessGrassButton = lessGrassButton;
        prevLessBushesButton = lessBushesButton;
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

                        if (dis < 3 && p.GetComponent<Predator>().canBreed && targeter.GetComponent<Predator>().canBreed)
                        {
                            targeter.GetComponent<Predator>().hunger /= 1.5f;
                            if (targeter.GetComponent<Predator>().hunger >= targeter.GetComponent<Predator>().breedOn) targeter.GetComponent<Predator>().hunger = targeter.GetComponent<Predator>().breedOn - 1;
                            targeter.GetComponent<Predator>().goBreed = false;
                            targeter.GetComponent<Predator>().curBreedCounter = targeter.GetComponent<Predator>().maxBreedCounter;
                            targeter.GetComponent<Predator>().canBreed = false;
                            p.GetComponent<Predator>().hunger /= 1.5f;
                            if (p.GetComponent<Predator>().hunger >= p.GetComponent<Predator>().breedOn) p.GetComponent<Predator>().hunger = p.GetComponent<Predator>().breedOn - 1;
                            p.GetComponent<Predator>().goBreed = false;
                            p.GetComponent<Predator>().curBreedCounter = p.GetComponent<Predator>().maxBreedCounter;
                            p.GetComponent<Predator>().canBreed = false;

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
                        targeter.GetComponent<Predator>().hunger += p.GetComponent<Prey>().mass * 10;
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
            else if (targeter.GetComponent<Prey>().goBreed && targeter.GetComponent<Prey>().canBreed)
            {
                foreach (GameObject p in allPrey)
                {
                    if (p.GetComponent<Prey>().goBreed && p.GetComponent<Prey>().canBreed && p != targeter)
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

                        if (dis < 3 && p.GetComponent<Prey>().canBreed && targeter.GetComponent<Prey>().canBreed)
                        {
                            targeter.GetComponent<Prey>().hunger /= 1.5f;
                            if (targeter.GetComponent<Prey>().hunger >= targeter.GetComponent<Prey>().breedOn) targeter.GetComponent<Prey>().hunger = targeter.GetComponent<Prey>().breedOn - 1;
                            targeter.GetComponent<Prey>().goBreed = false;
                            targeter.GetComponent<Prey>().curBreedCounter = targeter.GetComponent<Prey>().maxBreedCounter;
                            targeter.GetComponent<Prey>().canBreed = false;
                            p.GetComponent<Prey>().hunger /= 1.5f;
                            if (p.GetComponent<Prey>().hunger >= p.GetComponent<Prey>().breedOn) p.GetComponent<Prey>().hunger = p.GetComponent<Prey>().breedOn - 1;
                            p.GetComponent<Prey>().goBreed = false;
                            p.GetComponent<Prey>().curBreedCounter = p.GetComponent<Prey>().maxBreedCounter;
                            p.GetComponent<Prey>().canBreed = false;

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
