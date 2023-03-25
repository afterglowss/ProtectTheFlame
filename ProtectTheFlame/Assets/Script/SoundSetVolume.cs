using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundSetVolume : MonoBehaviour
{
    Slider soundSlider;
    private AudioSource soundSource;
    public AudioClip soundClipStep;
    public AudioClip soundClipWalk;

    private void Awake()
    {
        soundSlider = GetComponent<Slider>();
        soundSource = SoundManager.instance.GetComponentInChildren<AudioSource>();
    }
    void Start()
    {
        soundSlider.value = SoundManager.volume;
    }

    public void SetSoundVolume(float volume)
    {
        SoundManager.instance.SetSoundVolume(volume);
    }

    public void PlayClickSound()
    {
        if (!soundSource.isPlaying)
        {
            soundSource.clip = soundClipStep;
            soundSource.Play();
        }
    }
    public void PlayWalkSound()
    {
        if (!soundSource.isPlaying)
        {
            soundSource.clip = soundClipWalk;
            soundSource.Play();
        }
    }
}
