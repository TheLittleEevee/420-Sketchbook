using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.Return) && other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().gold += Random.Range(1, 10);
            Destroy(this.gameObject);
        }
    }
}
