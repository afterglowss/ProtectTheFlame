using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;
    [System.Serializable]
    public struct SoundType
    {
        public string name;
        public AudioClip sound;
        public float pitch;
    }

    public SoundType[] SoundList;

    public static float volume = 0.3f;
    public static SoundManager instance;
    public void SetSoundVolume(float volume)
    {
        SoundManager.volume = volume;
        soundSource.volume = volume;
    }

    public float GetSoundVolume()
    {
        return soundSource.volume;
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
    [YarnCommand("playSound")]
    public void PlaySound(string str)
    {
        for (int i = 0; i < SoundList.Length; i++)
        {
            if (str == SoundList[i].name)
            {
                soundSource.clip = SoundList[i].sound;
                if (!soundSource.isPlaying)
                {
                    soundSource.pitch = SoundList[i].pitch;
                    soundSource.Play();
                }
                break;
            }
        }
    }
    [YarnCommand("playRepeatSound")]
    public void PlayRepeatSound(string str)
    {
        for (int i = 0; i < SoundList.Length; i++)
        {
            if (str == SoundList[i].name)
            {
                soundSource.clip = SoundList[i].sound;
                if (!soundSource.isPlaying)
                {
                    soundSource.loop = true;
                    soundSource.pitch = SoundList[i].pitch;
                    soundSource.Play();
                }
                break;
            }
        }
    }
    [YarnCommand("stopSound")]
    public void StopSound()
    {
        soundSource.loop = false;
        soundSource.Stop();
    }
    [YarnCommand("unPauseSound")]
    public void UnPauseSound()
    {
        soundSource.UnPause();
    }


    private float previousVolume;

    [YarnCommand("soundFadeOut")]
    public void SoundFadeOut()
    {
        previousVolume = GetSoundVolume();
        StartCoroutine(SoundFadeOutCoroutine());
    }
    IEnumerator SoundFadeOutCoroutine()
    {
        float FadeCount = previousVolume;
        while (FadeCount > 0)
        {
            FadeCount -= 0.01f;
            yield return new WaitForSeconds(0.0005f);
            SetSoundVolume(FadeCount);
        }
        StopSound();
        SetSoundVolume(previousVolume);
    }
    [YarnCommand("soundFadeIn")]
    public void SoundFadeIn()
    {
        previousVolume = GetSoundVolume();
        StartCoroutine(SoundFadeInCoroutine());
    }
    IEnumerator SoundFadeInCoroutine()
    {
        float FadeCount = 0;
        UnPauseSound();
        while (FadeCount < previousVolume)
        {
            FadeCount += 0.005f;
            yield return new WaitForSeconds(0.0005f);
            SetSoundVolume(FadeCount);
        }
    }
}
