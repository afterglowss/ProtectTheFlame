using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class FlameSliderController : MonoBehaviour
{
    [HideInInspector]
    public static float FlameGage = 500f;
    [HideInInspector]
    public static bool stopFlameGage = false;
    
    void Update()
    {
        if (FlameGage < 0f)
        {
            FlameGage = 0;
        }
        else if (FlameGage > 1000f)
        {
            FlameGage = 1000;
        }
        FlameGUpDown();
    }

    public void FlameGUpDown()
    {
        if (!stopFlameGage)
        {
            FlameGage -= Time.deltaTime * 10;
        }
        GetComponent<Slider>().value = (int)FlameGage;
    }
}
