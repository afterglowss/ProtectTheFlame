using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSetVolume : MonoBehaviour
{
    Slider musicSlider;
    public void Awake()
    {
        musicSlider = GetComponent<Slider>();
    }
    private void Start()
    {
        musicSlider.value = MusicManager.volume;
    }
    public void SetMusicVolume(float volume)
    {
        MusicManager.instance.SetMusicVolume(volume);
    }
}
