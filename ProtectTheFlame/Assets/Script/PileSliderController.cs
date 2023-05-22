using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PileSliderController : MonoBehaviour
{
    [HideInInspector]
    public static float PileGage;
    [HideInInspector]
    public static bool stopPileGage;
    [HideInInspector]
    public static bool goOutPile;

    private void Awake()
    {
        PileGage = 500;
        stopPileGage = false;
        goOutPile = false;
    }

    void Update()
    {
        if (PileGage < 0)
        {
            PileGage = 0;
            goOutPile = true;
        }
        else if (PileGage > 1000)
        {
            PileGage = 1000;
        }
        PileGUpDown();
    }
    public void PileGUpDown()
    {
        if (!stopPileGage && !FlameSliderController.goOutFlame)     //불꽃이 꺼지면 장작 게이지 더이상 줄어들지 않음.
        {
            PileGage -= Time.deltaTime * 15;
        }
        GetComponent<Slider>().value = (int)PileGage;
    }
}
