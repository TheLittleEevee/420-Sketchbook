using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyTerrainType
{
    Open,
    Wall,
    Water,
    Lava,
    Spike,
    Tower,
    Spawner
}

public class MyTerrainCube : MonoBehaviour
{
    public Transform wall;
    public Transform water;
    public Transform lava;
    public Transform spike;
    public Transform tower;
    public Transform spawner;

    BoxCollider box;

    public MyTerrainType type = MyTerrainType.Open;

    public float MoveCost
    {
        get
        {
            if (type == MyTerrainType.Open) return 1;
            if (type == MyTerrainType.Wall) return 9999;
            if (type == MyTerrainType.Water) return 10;
            if (type == MyTerrainType.Lava) return 20;
            if (type == MyTerrainType.Spike) return 5;
            if (type == MyTerrainType.Tower) return 999;
            if (type == MyTerrainType.Spawner) return 1;
            return 1;
        }
    }

    public float PainCaused
    {
        get
        {
            if (type == MyTerrainType.Open) return 0;
            if (type == MyTerrainType.Wall) return 0;
            if (type == MyTerrainType.Water) return 0;
            if (type == MyTerrainType.Lava) return 20;
            if (type == MyTerrainType.Spike) return 5;
            if (type == MyTerrainType.Tower) return 0;
            if (type == MyTerrainType.Spawner) return 0;
            return 0;
        }
    }

    public float Health
    {
        get
        {
            if (type == MyTerrainType.Open) return 9999;
            if (type == MyTerrainType.Wall) return 50;
            if (type == MyTerrainType.Water) return 9999;
            if (type == MyTerrainType.Lava) return 9999;
            if (type == MyTerrainType.Spike) return 9999;
            if (type == MyTerrainType.Tower) return 500;
            if (type == MyTerrainType.Spawner) return 9999;
            return 9999;
        }
    }

    public float curHealth;

    public int thisPrice
    {
        get
        {
            if (type == MyTerrainType.Open) return 0;
            if (type == MyTerrainType.Wall) return 20;
            if (type == MyTerrainType.Water) return 10;
            if (type == MyTerrainType.Lava) return 60;
            if (type == MyTerrainType.Spike) return 5;
            if (type == MyTerrainType.Tower) return 0;
            if (type == MyTerrainType.Spawner) return 0;
            return 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curHealth = Health;
        box = GetComponent<BoxCollider>();
        UpdateArt();
    }

    // Update is called once per frame
    void Update()
    {
        if (type == MyTerrainType.Tower)
        {
            MyGrid.singleton.towerHealth = (int)Mathf.Ceil(curHealth);
        }

        if (curHealth <= 0)
        {
            if (type == MyTerrainType.Tower)
            {
                MyGrid.singleton.isLose = true;
                MyGrid.singleton.isPaused = true;
            }
            else
            {
                type = MyTerrainType.Open;

                //Change this TerrainCube's artwork
                UpdateArt();

                curHealth = Health;

                //Rebuild our array of nodes
                if (MyGrid.singleton) MyGrid.singleton.MakeNodes();
            }
        }
    }

    void UpdateArt()
    {
        if (type == MyTerrainType.Wall)
        {
            box.size = new Vector3(1, 1.1f, 1);
            box.center = new Vector3(0, .44f, 0);

        }
        else
        {
            box.size = new Vector3(1, .2f, 1);
            box.center = new Vector3(0, 0f, 0);
        }

        if (wall) wall.gameObject.SetActive(type == MyTerrainType.Wall);
        if (water) water.gameObject.SetActive(type == MyTerrainType.Water);
        if (lava) lava.gameObject.SetActive(type == MyTerrainType.Lava);
        if (spike) spike.gameObject.SetActive(type == MyTerrainType.Spike);
        if (tower) tower.gameObject.SetActive(type == MyTerrainType.Tower);
        if (spawner) spawner.gameObject.SetActive(type == MyTerrainType.Spawner);
    }

    void OnMouseDown() //Automatically called when object is clicked on as long as it has a collider
    {
        if (type == MyTerrainType.Tower || type == MyTerrainType.Spawner) return;

        if (MyGrid.singleton.price > MyGrid.singleton.money) return;

        //Change this TerrainCube's state (wall/water/none/etc)
        if ((int)type == MyGrid.singleton.activeCubeSwap) return;
        else
        {
            MyGrid.singleton.money += thisPrice;
            type = (MyTerrainType)MyGrid.singleton.activeCubeSwap;
            MyGrid.singleton.money -= MyGrid.singleton.price;
        }

        curHealth = Health;

        //Change this TerrainCube's artwork
        UpdateArt();

        //Rebuild our array of nodes
        if (MyGrid.singleton) MyGrid.singleton.MakeNodes();
    }
}
