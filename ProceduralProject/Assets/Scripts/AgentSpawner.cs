using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public GameObject bigFishPrefab;

    private List<GameObject> agents = new List<GameObject>();
    private GameObject b;
    // Start is called before the first frame update
    void Start()
    {
        b = Instantiate(bigFishPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (agents.Count < 20)
        {
            GameObject a = Instantiate(agentPrefab);
            agents.Add(a);
        }
    }
}
