using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemparatureSliderController : MonoBehaviour
{
    [HideInInspector]
    public float TemparatureGage = 70;
    public bool stopSlider;


    private void Start()
    {
        stopSlider = false;
    }

    void Update()
    {
        if (TemparatureGage < 0)
        {
            TemparatureGage = 0;
        }
        if (TemparatureGage > 100)
        {
            TemparatureGage = 100;
        }
        TempGUpDown();
    }
     
    public void TempGUpDown()
    {
        if (!stopSlider)
        {
            TemparatureGage -= Time.deltaTime * 2;
        }
        else if (stopSlider)
        {
            TemparatureGage += Time.deltaTime * 4;

        }
        GetComponent<Slider>().value = (int)TemparatureGage;
    }
}
