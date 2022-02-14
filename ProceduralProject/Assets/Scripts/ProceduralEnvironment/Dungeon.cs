using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject voxelPrefab;

    //bool pressedRegen = false;
    //bool prevPressedRegen = false;

    int roomSize = 10;
    int res = 50;
    int[,] rooms;

    int lilPerBig = 5;
    int lowres() { return res / lilPerBig; }
    int[,] bigrooms;

    public GameObject player;
    public GameObject cameraTarget;
    PlayerController playerController;

    public GameObject portalStart;
    public GameObject portalEnd;

    private float portalStartX = 0;
    private float portalStartZ = 0;
    private float portalEndX = 0;
    private float portalEndZ = 0;

    public GameObject plant;
    public GameObject rock;
    public GameObject chest;
    public GameObject fish;

    public int floorNum;

    // Start is called before the first frame update
    void Start()
    {
        floorNum = 1;
        playerController = player.GetComponent<PlayerController>();

        generate();
        
    }

    void setRoom(int x, int y, int t)
    {
        //Check for Errors
        if (x < 0) return;
        if (y < 0) return;
        if (x >= rooms.GetLength(0)) return;
        if (y >= rooms.GetLength(1)) return;

        int temp = getRoom(x, y);
        if (temp < t) rooms[x, y] = t;
    }

    void setBigRoom(int x, int y, int t)
    {
        //Check for Errors
        if (x < 0) return;
        if (y < 0) return;
        if (x >= bigrooms.GetLength(0)) return;
        if (y >= bigrooms.GetLength(1)) return;

        bigrooms[x, y] = t;
    }

    int getRoom(int x, int y)
    {
        //Check for Errors
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= rooms.GetLength(0)) return 0;
        if (y >= rooms.GetLength(1)) return 0;

        return rooms[x, y];
    }

    int getBigRoom(int x, int y)
    {
        //Check for Errors
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= bigrooms.GetLength(0)) return 0;
        if (y >= bigrooms.GetLength(1)) return 0;

        return bigrooms[x, y];
    }

    void generate()
    {
        rooms = new int[res, res];

        walkRooms(3, 4);
        walkRooms(2, 2);
        walkRooms(2, 2);
        walkRooms(2, 2);

        makeBigRooms();
        punchHoles();
        spawnCubes();
    }

    void punchHoles()
    {
        for (int x = 0; x < bigrooms.GetLength(0); x++)
        {
            for (int y = 0; y < bigrooms.GetLength(1); y++)
            {
                int val = getBigRoom(x, y);
                if (val != 1) continue; //Only consider rooms of value 1

                if (Random.Range(0, 100) < 25) continue; //25% of time, don't punch holes

                int[] neighbors = new int[8];

                neighbors[0] = getBigRoom(x, y - 1); //Above
                neighbors[1] = getBigRoom(x + 1, y - 1);
                neighbors[2] = getBigRoom(x + 1, y); //Right
                neighbors[3] = getBigRoom(x + 1, y + 1);
                neighbors[4] = getBigRoom(x, y + 1); //Below
                neighbors[5] = getBigRoom(x - 1, y + 1);
                neighbors[6] = getBigRoom(x - 1, y); //Left
                neighbors[7] = getBigRoom(x - 1, y - 1);

                bool isSolid = neighbors[7] > 0;
                int tally = 0;

                foreach (int n in neighbors)
                {
                    bool s = n > 0;

                    if (s != isSolid) tally++;

                    isSolid = s;
                }
                if (tally <= 2)
                {
                    //Safe to punch a hole
                    setBigRoom(x, y, 0);
                }
            }
        }
    }

    void makeBigRooms()
    {
        int r = lowres();
        bigrooms = new int[r, r];

        for (int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                int val1 = getRoom(x, y);
                int val2 = bigrooms[(x / lilPerBig), (y / lilPerBig)];

                if (val1 > val2)
                {
                    bigrooms[(x / lilPerBig), (y / lilPerBig)] = val1;
                }
            }
        }
    }

    void walkRooms(int type1, int type2)
    {
        //Walking
        int halfW = rooms.GetLength(0) / 2;
        int halfH = rooms.GetLength(1) / 2;

        int x = (int)Random.Range(0, rooms.GetLength(0));
        int y = (int)Random.Range(0, rooms.GetLength(1));
        int tx = (int)Random.Range(0, halfW);
        int ty = (int)Random.Range(0, halfH);

        if (x < halfW) tx += halfW; //If starting point on left, move end point to right
        if (y < halfH) ty += halfH; //Move end to bottom half of dungeon

        setRoom(x, y, type1);
        setRoom(tx, ty, type2);

        int n = 0;

        while (x != tx || y != ty)
        {
            int dir = (int)Random.Range(0, 4); //0 to 3
            int dis = (int)Random.Range(1, 4); //1 to 3

            n++;

            if (Random.Range(0, 100) > 50)
            {
                int dx = tx - x;
                int dy = ty - y;

                if (Mathf.Abs(dx) < Mathf.Abs(dy))
                { // We are closer on x-axis than y
                    dir = (dy < 0) ? 0 : 1;
                }
                else
                { // We are closer on y-axis than x
                    dir = (dx < 0) ? 3 : 2;
                }
            }



            for (int i = 0; i < dis; i++)
            {
                switch (dir)
                {
                    case 0:
                        y--; //Move north
                        break;
                    case 1:
                        y++; //Move south
                        break;
                    case 2:
                        x++; //Move east
                        break;
                    case 3: //Move west
                        x--;
                        break;
                }
                if (x < 0) x = 0;
                if (x > res - 1) x = res - 1;
                if (y < 0) y = 0;
                if (y > res - 1) y = res - 1;

                setRoom(x, y, 1);

            } //End for
        } //End while
    }

    void spawnCubes()
    {
        if (!voxelPrefab) return;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        float px = roomSize;
        for (int x = 0; x < rooms.GetLength(0); x++)
        { //Length of the first dimension (x)
            for (int z = 0; z < rooms.GetLength(1); z++)
            { //Length of second dimension (y)
                int val = rooms[x, z];
                if (val > 0)
                {
                    switch (val)
                    {
                        case 1:
                            break;
                        case 2:
                            //Vector3 pos2 = new Vector3(x, 1, z) * px;
                            //GameObject obj2 = Instantiate(voxelPrefab, pos2, Quaternion.identity, transform);
                            //obj2.transform.localScale = Vector3.one * px / 2;
                            break;
                        case 3:
                            //Vector3 pos3 = new Vector3(x, 1, z) * px;
                            //GameObject obj3 = Instantiate(voxelPrefab, pos3, Quaternion.identity, transform);
                            //obj3.transform.localScale = Vector3.one * px;
                            break;
                        case 4:
                            //Vector3 pos4 = new Vector3(x, 1, z) * px;
                            //GameObject obj4 = Instantiate(voxelPrefab, pos4, Quaternion.identity, transform);
                            //obj4.transform.localScale = Vector3.one * px / 4;
                            break;
                    }
                    //Vector3 pos = new Vector3(x, 0, z) * px;
                    //GameObject obj = Instantiate(voxelPrefab, pos, Quaternion.identity, transform);
                    //obj.transform.localScale = Vector3.one * px;
                }
            }
        }

        px = roomSize * lilPerBig;
        for (int x = 0; x < bigrooms.GetLength(0); x++)
        { //Length of the first dimension (x)
            for (int z = 0; z < bigrooms.GetLength(1); z++)
            { //Length of second dimension (y)
                int val = bigrooms[x, z];
                if (val > 0)
                {
                    Vector3 pos = new Vector3(x, 0, z) * px;
                    GameObject obj = Instantiate(voxelPrefab, pos, Quaternion.identity, transform);
                    obj.transform.localScale = Vector3.one * px;
                    switch (val)
                    {
                        case 1:
                            float xP1 = Random.Range(x - 20/px, x + 20/px);
                            float zP1 = Random.Range(z - 20/px, z + 20/px);
                            plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                            plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                            Vector3 posP1 = new Vector3(xP1, .5f, zP1) * px;
                            GameObject objP1 = Instantiate(plant, posP1, Quaternion.identity, transform);

                            float xR1 = Random.Range(x - 20 / px, x + 20 / px);
                            float zR1 = Random.Range(z - 20 / px, z + 20 / px);
                            Vector3 posR1 = new Vector3(xR1, .5f, zR1) * px;
                            GameObject objR1 = Instantiate(rock, posR1, Quaternion.identity, transform);
                            objR1.transform.localScale *= Random.Range(0.5f, 2);

                            if (Random.Range(0, 100) > 65)
                            {
                                if (Random.Range(0, 100) > 50)
                                {
                                    float xP2 = Random.Range(x - 20 / px, x + 20 / px);
                                    float zP2 = Random.Range(z - 20 / px, z + 20 / px);
                                    plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                                    plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                                    Vector3 posP2 = new Vector3(xP2, .5f, zP2) * px;
                                    GameObject objP2 = Instantiate(plant, posP2, Quaternion.identity, transform);
                                }
                                else
                                {
                                    float xR2 = Random.Range(x - 20 / px, x + 20 / px);
                                    float zR2 = Random.Range(z - 20 / px, z + 20 / px);
                                    Vector3 posR2 = new Vector3(xR2, .5f, zR2) * px;
                                    GameObject objR2 = Instantiate(rock, posR2, Quaternion.identity, transform);
                                    objR2.transform.localScale *= Random.Range(0.5f, 2);
                                }
                            }
                            if (Random.Range(0, 100) > 85)
                            {
                                if (Random.Range(0, 100) > 75)
                                {
                                    float xP3 = Random.Range(x - 20 / px, x + 20 / px);
                                    float zP3 = Random.Range(z - 20 / px, z + 20 / px);
                                    plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                                    plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                                    Vector3 posP3 = new Vector3(xP3, .5f, zP3) * px;
                                    GameObject objP3 = Instantiate(plant, posP3, Quaternion.identity, transform);
                                }
                                else
                                {
                                    float xR3 = Random.Range(x - 20 / px, x + 20 / px);
                                    float zR3 = Random.Range(z - 20 / px, z + 20 / px);
                                    Vector3 posR3 = new Vector3(xR3, .5f, zR3) * px;
                                    GameObject objR3 = Instantiate(rock, posR3, Quaternion.identity, transform);
                                    objR3.transform.localScale *= Random.Range(0.5f, 2);
                                }
                            }
                            break;
                        case 2:
                            Vector3 pos2 = new Vector3(x, 1f, z) * px - new Vector3(0, 22.75f, 0);
                            GameObject obj2 = Instantiate(chest, pos2, Quaternion.identity, transform);

                            if (Random.Range(0, 100) > 50)
                            {
                                float xP2 = Random.Range(x - 20 / px, x + 20 / px);
                                if (xP2 >= x && xP2 < x + 10 / px) xP2 = x + 10 / px;
                                if (xP2 < x && xP2 > x - 10 / px) xP2 = x - 10 / px;
                                float zP2 = Random.Range(z - 20 / px, z + 20 / px);
                                plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                                plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                                Vector3 posP2 = new Vector3(xP2, .5f, zP2) * px;
                                GameObject objP2 = Instantiate(plant, posP2, Quaternion.identity, transform);
                            }
                            else
                            {
                                float xR2 = Random.Range(x - 20 / px, x + 20 / px);
                                float zR2 = Random.Range(z - 20 / px, z + 20 / px);
                                if (xR2 >= x && xR2 < x + 10 / px) xR2 = x + 10 / px;
                                if (xR2 < x && xR2 > x - 10 / px) xR2 = x - 10 / px;
                                Vector3 posR2 = new Vector3(xR2, .5f, zR2) * px;
                                GameObject objR2 = Instantiate(rock, posR2, Quaternion.identity, transform);
                                objR2.transform.localScale *= Random.Range(0.5f, 2);
                            }
                            break;
                        case 3:
                            playerController.body.position = new Vector3(x, 1, z) * px - new Vector3(0, 20, 0);

                            Vector3 pos3 = new Vector3(x, .5f, z) * px;
                            portalStartX = x * px;
                            portalStartZ = z * px;
                            GameObject obj3 = Instantiate(portalStart, pos3, Quaternion.identity, transform);

                            if (Random.Range(0, 100) > 50)
                            {
                                float xP2 = Random.Range(x - 20 / px, x + 20 / px);
                                if (xP2 >= x && xP2 < x + 10 / px) xP2 = x + 10 / px;
                                if (xP2 < x && xP2 > x - 10 / px) xP2 = x - 10 / px;
                                float zP2 = Random.Range(z - 20 / px, z + 20 / px);
                                plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                                plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                                Vector3 posP2 = new Vector3(xP2, .5f, zP2) * px;
                                GameObject objP2 = Instantiate(plant, posP2, Quaternion.identity, transform);
                            }
                            else
                            {
                                float xR2 = Random.Range(x - 20 / px, x + 20 / px);
                                float zR2 = Random.Range(z - 20 / px, z + 20 / px);
                                if (xR2 >= x && xR2 < x + 10 / px) xR2 = x + 10 / px;
                                if (xR2 < x && xR2 > x - 10 / px) xR2 = x - 10 / px;
                                Vector3 posR2 = new Vector3(xR2, .5f, zR2) * px;
                                GameObject objR2 = Instantiate(rock, posR2, Quaternion.identity, transform);
                                objR2.transform.localScale *= Random.Range(0.5f, 2);
                            }
                            break;
                        case 4:
                            Vector3 pos4 = new Vector3(x, .5f, z) * px;
                            portalEndX = x * px;
                            portalEndZ = z * px;
                            GameObject obj4 = Instantiate(portalEnd, pos4, Quaternion.identity, transform);

                            if (Random.Range(0, 100) > 50)
                            {
                                float xP2 = Random.Range(x - 20 / px, x + 20 / px);
                                if (xP2 >= x && xP2 < x + 10 / px) xP2 = x + 10 / px;
                                if (xP2 < x && xP2 > x - 10 / px) xP2 = x - 10 / px;
                                float zP2 = Random.Range(z - 20 / px, z + 20 / px);
                                plant.GetComponent<MyPlant>().iterations = Random.Range(2, 11);
                                plant.GetComponent<MyPlant>().spreadDegrees = Random.Range(5, 30);
                                Vector3 posP2 = new Vector3(xP2, .5f, zP2) * px;
                                GameObject objP2 = Instantiate(plant, posP2, Quaternion.identity, transform);
                            }
                            else
                            {
                                float xR2 = Random.Range(x - 20 / px, x + 20 / px);
                                float zR2 = Random.Range(z - 20 / px, z + 20 / px);
                                if (xR2 >= x && xR2 < x + 10 / px) xR2 = x + 10 / px;
                                if (xR2 < x && xR2 > x - 10 / px) xR2 = x - 10 / px;
                                Vector3 posR2 = new Vector3(xR2, .5f, zR2) * px;
                                GameObject objR2 = Instantiate(rock, posR2, Quaternion.identity, transform);
                                objR2.transform.localScale *= Random.Range(0.5f, 2);
                            }
                            break;
                    }
                }
                else
                {
                    if (Random.Range(0, 100) > 25)
                    {
                        float xF1 = Random.Range(x - 20 / px, x + 20 / px);
                        float zF1 = Random.Range(z - 20 / px, z + 20 / px);
                        Vector3 posF1 = new Vector3(xF1 * px, 19.5f, zF1 * px);
                        GameObject objF1 = Instantiate(fish, posF1, Quaternion.Euler(90, Random.Range(0, 360), 0), transform);
                    }
                    if (Random.Range(0, 100) > 80)
                    {
                        float xF1 = Random.Range(x - 20 / px, x + 20 / px);
                        float zF1 = Random.Range(z - 20 / px, z + 20 / px);
                        Vector3 posF1 = new Vector3(xF1 * px, 19.5f, zF1 * px);
                        GameObject objF1 = Instantiate(fish, posF1, Quaternion.Euler(90, Random.Range(0, 360), 0), transform);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.body.position.x >= portalEndX - 5 && playerController.body.position.x <= portalEndX + 5 && playerController.body.position.z >= portalEndZ - 5 && playerController.body.position.z <= portalEndZ + 5)
        {
            floorNum++;
            generate();
        }

        if (playerController.body.position.y <= 25)
        {
            playerController.body.position = new Vector3(portalStartX, 30, portalStartZ);
            playerController.gold -= 1;
            playerController.gold -= (int)Random.Range((float)playerController.gold * .1f, (float)playerController.gold * .3f);
            if (playerController.gold < 0) playerController.gold = 0;
        }
    }
}