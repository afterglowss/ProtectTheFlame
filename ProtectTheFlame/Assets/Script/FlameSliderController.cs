using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class FlameSliderController : MonoBehaviour
{
    [HideInInspector]
    public float FlameGage = 500f;
    [HideInInspector]
    public bool StopFlameGage = false;
    
    void Update()
    {
        if (!StopFlameGage)
        {
            if (FlameGage < 0f)
            {
                FlameGage = 0;
            }
            else if (FlameGage > 1000f)
            {
                FlameGage = 1000;
            }
            else
            {
                FlameGage -= Time.deltaTime * 10;
                GetComponent<Slider>().value = (int)FlameGage;
            }
        }
    }
}
