using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PileSliderController : MonoBehaviour
{
    [HideInInspector]
    public float PileGage = 500;

    void Update()
    {
        if (PileGage < 0)
        {
            PileGage = 0;
        }
        else if (PileGage > 1000)
        {
            PileGage = 1000;
        }
        else
        {
            PileGage -= Time.deltaTime * 10;
            GetComponent<Slider>().value = (int)PileGage;
        }
    }
}
