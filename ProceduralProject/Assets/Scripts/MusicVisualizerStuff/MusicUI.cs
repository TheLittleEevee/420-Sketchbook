using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    public Slider slider;

    public MyMusicPlayer mPlayer;
    public MyPostProcessing ppShader;
    public MyGravitation grav;
    public MyVisualizer visualizer;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();

        slider.onValueChanged.AddListener(OnSliderChange);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = ppShader.distortion;
    }

    public void OnSliderChange(float value)
    {
        ppShader.distortion = value;
        ppShader.UpdateDistortion(value);
    }

    public void OnButtonNext()
    {
        mPlayer.musicPlayer.Stop();
        mPlayer.PlayTrackNext();
    }

    public void OnButtonMore()
    {
        grav.maxBoids++;
    }

    public void OnButtonLess()
    {
        grav.maxBoids--;
        if (grav.maxBoids <= 1) grav.maxBoids = 1;
    }

    public void OnButtonRegen()
    {
        visualizer.Reorder();
    }
}
