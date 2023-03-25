using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileSliderController : MonoBehaviour
{
    public float PileGage = 500;

    void Update()
    {
        PileGage -= Time.deltaTime * 10;
        GetComponent<UnityEngine.UI.Slider>().value = (int)PileGage;
    }
}
