using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject chunk;

    public GameObject player;

    [SerializeField]
    private TextMeshProUGUI floorNumText;

    [SerializeField]
    private TextMeshProUGUI goldText;

    // Start is called before the first frame update
    void Start()
    {
        floorNumText.text = "Floor: " + chunk.GetComponent<Dungeon>().floorNum.ToString();
        goldText.text = "Gold: " + player.GetComponent<PlayerController>().gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        floorNumText.text = "Floor: " + chunk.GetComponent<Dungeon>().floorNum.ToString();
        goldText.text = "Gold: " + player.GetComponent<PlayerController>().gold.ToString();
    }

    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 40, 20), "Test"))
        {
            Debug.Log("Test button");
        }
    }
    */
}
