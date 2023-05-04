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
    public static bool goOutFlame;          //�Ҳ��� �������� Ȯ���� �� ����

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
        if (PileSliderController.goOutPile && !goOutFlame)     //������ �� ��Ҵٸ� ������ ��ġ ���ο� ������� ������ ���
        {
            FlameGage -= Time.deltaTime * 30;   //�� 4�踸ŭ ��� ���.
        }
        GetComponent<Slider>().value = (int)FlameGage;
    }
}
