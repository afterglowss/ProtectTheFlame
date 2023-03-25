using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;

    public static float volume = 0.5f;
    public static SoundManager instance;
    public void SetSoundVolume(float volume)
    {
        SoundManager.volume = volume;
        soundSource.volume = volume;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
