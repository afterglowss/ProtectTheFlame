using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemparatureSliderController : MonoBehaviour
{
    public float TemparatureGage = 70;
    public bool stopSlider;

    private void Start()
    {
        stopSlider = false;
    }

    void Update()
    {
        if (!stopSlider)
        {
            TemparatureGage -= Time.deltaTime * 2;
            GetComponent<UnityEngine.UI.Slider>().value = (int)TemparatureGage;
        }
        else if (stopSlider)
        {
            TemparatureGage += Time.deltaTime * 1;
            GetComponent<UnityEngine.UI.Slider>().value = (int)TemparatureGage;
        }
    }
            
}
