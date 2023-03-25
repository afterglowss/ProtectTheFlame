using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;


public class FlameSliderController : MonoBehaviour
{
    public float FlameGage=500;
    
    void Update()
    {
        FlameGage -= Time.deltaTime * 10;
        GetComponent<UnityEngine.UI.Slider>().value = (int)FlameGage;
    }
}
