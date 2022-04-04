using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    delegate MyPathfinder.Node LookupDelegate(int x, int y);

    public static MyGrid singleton { get; private set; }

    public MyTerrainCube cubePrefab;
    public GameObject enemyPrefab;

    public Transform helperStart1;
    public Transform helperStart2;
    public Transform helperStart3;
    public Transform helperEnd;

    private MyTerrainCube[,] cubes;
    private MyPathfinder.Node[,] nodes;

    public int activeCubeSwap = 0;
    public int price = 0;
    public int money = 100;
    private float moneyCounter = 0;
    public int towerHealth;

    public bool isPaused = true;
    public int pausesLeft = 2;
    public bool inWave = false;
    public int waveNum = 1;
    public int enemyMaxHealth = 100;
    public bool isWin = false;
    public bool isLose = false;

    private float enemySpawner = 100;
    public int totalEnemiesInWave = 0;
    public int curEnemiesInWave = 0;
    public static List<GameObject> allEnemies = new List<GameObject>();
    public static List<GameObject> destroyQueue = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null) //We already have a singleton
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;

        MakeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            moneyCounter += Time.deltaTime;

            if (moneyCounter >= 1)
            {
                money++;
                moneyCounter = 0;
            }

            if (curEnemiesInWave < totalEnemiesInWave)
            {
                enemySpawner += Time.deltaTime;
                if (enemySpawner >= 5)
                {
                    int randomChoice = Random.Range(0, 3);
                    if (randomChoice == 0) allEnemies.Add(Instantiate(enemyPrefab, new Vector3(helperStart1.position.x, 1.1f, helperStart1.position.z), Quaternion.identity));
                    if (randomChoice == 1) allEnemies.Add(Instantiate(enemyPrefab, new Vector3(helperStart2.position.x, 1.1f, helperStart2.position.z), Quaternion.identity));
                    if (randomChoice == 2) allEnemies.Add(Instantiate(enemyPrefab, new Vector3(helperStart3.position.x, 1.1f, helperStart3.position.z), Quaternion.identity));

                    enemySpawner = 0;
                    curEnemiesInWave++;
                }
            }
            else
            {
                if (allEnemies.Count == 0)
                {
                    waveNum++;

                    if (waveNum >= 6)
                    {
                        isWin = true;
                        isPaused = true;
                    }
                    else
                    {
                        isPaused = true;
                        inWave = false;
                        totalEnemiesInWave = 0;
                        curEnemiesInWave = 0;
                        enemySpawner = 100;
                    }
                }
            }
        }

        if (nodes != null)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    MyPathfinder.Node n = nodes[x, y];

                    if (n.health <= 0)
                    {
                        cubes[x, y].type = MyTerrainType.Open;
                    }
                }
            }
        }

         foreach (GameObject e in allEnemies)
        {
            if (e.GetComponent<Enemy>().isDead) destroyQueue.Add(e);
        }

        foreach (GameObject e in destroyQueue)
        {
            allEnemies.Remove(e);
            Destroy(e);
        }
    }

    void MakeGrid()
    {
        int size = 19;
        cubes = new MyTerrainCube[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                cubes[x, y] = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);

                if (y == size - 1 && x == (int)((size - 1)/2))
                {
                    cubes[x, y].type = MyTerrainType.Tower;
                    helperEnd = cubes[x, y].transform;
                }

                if (y == 0 && x == (int)((size - 1) / 2))
                {
                    cubes[x, y].type = MyTerrainType.Spawner;
                    helperStart1 = cubes[x, y].transform;
                }
                if (y == 0 && x == 0)
                {
                    cubes[x, y].type = MyTerrainType.Spawner;
                    helperStart2 = cubes[x, y].transform;
                }
                if (y == 0 && x == (size - 1))
                {
                    cubes[x, y].type = MyTerrainType.Spawner;
                    helperStart3 = cubes[x, y].transform;
                }
            }
        }
    }

    public void MakeNodes()
    {
        nodes = new MyPathfinder.Node[cubes.GetLength(0), cubes.GetLength(1)];

        for (int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                MyPathfinder.Node n = new MyPathfinder.Node();

                n.position = cubes[x, y].transform.position;
                n.moveCost = cubes[x, y].MoveCost;
                n.painCaused = cubes[x, y].PainCaused;
                n.health = cubes[x, y].curHealth;

                nodes[x, y] = n;
            }
        }

        LookupDelegate lookup = (x, y) => {
            if (x < 0) return null;
            if (y < 0) return null;
            if (x >= nodes.GetLength(0)) return null;
            if (y >= nodes.GetLength(1)) return null;
            return nodes[x, y];
        };

        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                MyPathfinder.Node n = nodes[x, y];

                //NonDiagonal
                MyPathfinder.Node neighbor1 = lookup(x + 1, y);
                MyPathfinder.Node neighbor2 = lookup(x - 1, y);
                MyPathfinder.Node neighbor3 = lookup(x, y + 1);
                MyPathfinder.Node neighbor4 = lookup(x, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);
            }
        }

        //Making a path through the "dungeon" using items in the world
        //MyPathfinder.Node start = Lookup(helperStart.position);
        //MyPathfinder.Node end = Lookup(helperEnd.position);

        //List<MyPathfinder.Node> path = MyPathfinder.Solve(start, end);
    }

    public MyPathfinder.Node Lookup(Vector3 pos)
    {
        if (nodes == null)
        {
            MakeNodes();
        }
        float w = 1;
        float h = 1;

        pos.x += w / 2;
        pos.z += h / 2;

        int x = (int)(pos.x / w); //Should truncate (round down if positive, but up if negative)
        int y = (int)(pos.z / h);

        if (x < 0 || y < 0) return null;
        if (x >= nodes.GetLength(0) || y >= nodes.GetLength(1)) return null;

        return nodes[x, y];
    }

    public MyTerrainCube LookupCube(MyPathfinder.Node node)
    {
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                if (node.position == cubes[x, y].transform.position) return cubes[x, y];
            }
        }

        return null;
    }

    void OnDestroy()
    {
        if (this == singleton) singleton = null;
    }
}
