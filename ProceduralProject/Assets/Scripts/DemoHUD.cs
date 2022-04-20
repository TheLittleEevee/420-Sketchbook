using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

public class DemoHUD : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>(); //Only works if you only have one slider

        if (PlayerPrefs.HasKey("The volume"))
        {
            slider.value = PlayerPrefs.GetFloat("The volume", 0); //Load a value
        }
        slider.onValueChanged.AddListener(OnSliderChange); //Do this if you don't want to do it in the inspector
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderChange(float value)
    {
        //To Do: Store in PlayerPrefs
        PlayerPrefs.SetFloat("The volume", value); //Save the value
        print($"value saved: {value}");
    }

    public void OnButtonSave()
    {
        SaveData data = new SaveData();
        data.playerHealth = 42;

        FileStream stream = File.OpenWrite("savegame.dagd420");

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(stream, data);

        stream.Close();
    }

    public void OnButtonLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = null;
        try
        {
            stream = File.OpenRead("savegame.dagd420");
        }
        catch (System.Exception e)
        {
            return;
        }

        SaveData data = null;
        
        try
        {
            data = (SaveData)bf.Deserialize(stream);
        }
        catch (System.Exception e)
        {

        }

        stream.Close();

        if (data != null) print(data.playerHealth);
    }
}
