using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Emergent : MonoBehaviour
{
    public EmergentBehavior eb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonMoreBushes()
    {
        eb.maxBushes++;
    }

    public void OnButtonLessBushes()
    {
        eb.maxBushes--;
        if (eb.maxBushes <= 0) eb.maxBushes = 0;
    }

    public void OnButtonMoreGrass()
    {
        eb.maxGrass++;
    }

    public void OnButtonLessGrass()
    {
        eb.maxGrass--;
        if (eb.maxGrass <= 0) eb.maxGrass = 0;
    }

    public void OnButtonMorePrey()
    {
        eb.SpawnNewPrey();
    }

    public void OnButtonMorePredators()
    {
        eb.SpawnNewPred();
    }
}
