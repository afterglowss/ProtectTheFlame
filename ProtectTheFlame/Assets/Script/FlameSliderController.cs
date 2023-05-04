using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class FlameSliderController : MonoBehaviour
{
    [HideInInspector]
    public static float FlameGage;
    [HideInInspector]
    public static bool stopFlameGage;
    [HideInInspector]
    public static bool goOutFlame;          //불꽃이 꺼졌는지 확인할 불 변수

    private void Awake()
    {
        FlameGage = 500f;
        stopFlameGage = false;
        goOutFlame = false;
    }

    void Update()
    {
        if (FlameGage < 0f)
        {
            FlameGage = 0;
            goOutFlame = true;
        }
        else if (FlameGage > 1000f)
        {
            FlameGage = 1000;
        }
        FlameGUpDown();
    }

    public void FlameGUpDown()
    {
        if (!stopFlameGage && !goOutFlame)
        {
            FlameGage -= Time.deltaTime * 10;
        }
        if (PileSliderController.goOutPile && !goOutFlame)     //장작이 다 닳았다면 가림막 설치 여부와 상관없이 빠르게 닳기
        {
            FlameGage -= Time.deltaTime * 30;   //총 4배만큼 계속 닳기.
        }
        GetComponent<Slider>().value = (int)FlameGage;
    }
}
