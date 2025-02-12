using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Open,
    Wall,
    Slime
}

public class TerrainCube : MonoBehaviour
{
    public Transform wall;
    public Transform slime;

    BoxCollider box;

    public TerrainType type = TerrainType.Open;

    public float MoveCost
    {
        get
        {
            if (type == TerrainType.Open) return 1;
            if (type == TerrainType.Wall) return 9999;
            if (type == TerrainType.Slime) return 10;
            return 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        UpdateArt();
    }

    void OnMouseDown() //Automatically called when object is clicked on as long as it has a collider
    {
        //Change this TerrainCube's state (wall/slime/none)
        type += 1;
        if ((int)type > 2) type = 0;

        //Change this TerrainCube's artwork
        UpdateArt();

        //Rebuild our array of nodes
        if (GridController.singleton) GridController.singleton.MakeNodes();
    }

    void UpdateArt()
    {
        bool isShowingWall = (type == TerrainType.Wall);

        float y = isShowingWall ? .44f : 0f;
        float h = isShowingWall ? 1.1f : .2f;
        box.size = new Vector3(1, h, 1);
        box.center = new Vector3(0, y, 0);

        if (wall) wall.gameObject.SetActive(isShowingWall);
        if (slime) slime.gameObject.SetActive(type == TerrainType.Slime);
    }
}
