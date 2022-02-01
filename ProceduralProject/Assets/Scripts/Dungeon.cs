using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject voxelPrefab;

    bool pressedRegen = false;
    bool prevPressedRegen = false;

    int roomSize = 10;
    int res = 50;
    int[,] rooms;

    int lilPerBig = 5;
    int lowres() { return res / lilPerBig; }
    int[,] bigrooms;

    // Start is called before the first frame update
    void Start()
    {
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

        //for (int x = 0; x < rooms.length; x++){
        //  for (int y = 0; y < rooms[x].length; y++){
        //    rooms[x][y] = (int)random(0, 5);
        //  }
        //}

        walkRooms(3, 4);
        walkRooms(2, 2);
        walkRooms(2, 2);
        walkRooms(2, 2);

        //Check for islands
        //...

        makeBigRooms();
        punchHoles();
        spawnCubes();

        //Spawn game objects
        //Room prefab

        print("Done generating");
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
                //x = constrain(x, 0, res - 1);
                //y = constrain(y, 0, res - 1);
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
                    Vector3 pos = new Vector3(x, 0, z) * px;
                    GameObject obj = Instantiate(voxelPrefab, pos, Quaternion.identity, transform);
                    obj.transform.localScale = Vector3.one * px;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return)) pressedRegen = true;
        else pressedRegen = false;

        if (pressedRegen && !prevPressedRegen)
        {
            generate();
        }
        prevPressedRegen = pressedRegen;
    }
}