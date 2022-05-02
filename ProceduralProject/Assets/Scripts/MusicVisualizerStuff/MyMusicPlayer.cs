using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyMusicPlayer : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource musicPlayer;

    private int currentTrack = -1;

    private bool nextButton = false;
    private bool prevNextButton = false;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        PlayTrackRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicPlayer.isPlaying)
        {
            PlayTrackNext();
        }

        nextButton = false;

        if (Input.GetKey(KeyCode.RightControl))
        {
            nextButton = true;
        }
        if (nextButton && !prevNextButton)
        {
            musicPlayer.Stop();
            PlayTrackNext();
        }

        prevNextButton = nextButton;
    }

    public void PlayTrack(int n)
    {
        if (n < 0 || n >= playlist.Length) return;
        musicPlayer.PlayOneShot(playlist[n]);
        currentTrack = n;
    }

    public void PlayTrackRandom()
    {
        PlayTrack(Random.Range(0, playlist.Length));
    }

    public void PlayTrackNext()
    {
        int track = currentTrack + 1;
        if (track >= playlist.Length) track = 0;

        PlayTrack(track);
    }
}
