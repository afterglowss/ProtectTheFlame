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
        if (!stopPileGage && !FlameSliderController.goOutFlame)     //�Ҳ��� ������ ���� ������ ���̻� �پ���� ����.
        {
            PileGage -= Time.deltaTime * 15;
        }
        GetComponent<Slider>().value = (int)PileGage;
    }
}
