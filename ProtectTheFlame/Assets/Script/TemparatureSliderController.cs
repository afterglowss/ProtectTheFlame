using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemparatureSliderController : MonoBehaviour
{
    [HideInInspector]
    public static float TemparatureGage;
    [HideInInspector]
    public static bool stopTemparatureGage;


    private void Start()
    {
        stopTemparatureGage = false;
        TemparatureGage = 70;
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
        if (!stopTemparatureGage)
        {
            TemparatureGage -= Time.deltaTime * 2;
        }
        else if (stopTemparatureGage)
        {
            TemparatureGage += Time.deltaTime * 4;

        }
        GetComponent<Slider>().value = (int)TemparatureGage;
    }
}
