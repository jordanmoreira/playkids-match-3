using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip clear;
    public AudioClip select;
    public AudioClip swap;

    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayClear()
    {
        audio.clip = clear;
        audio.Play();
    }
    public void PlaySelect()
    {
        audio.clip = select;
        audio.Play();
    }
    public void PlaySwap()
    {
        audio.clip = swap;
        audio.Play();
    }
}
