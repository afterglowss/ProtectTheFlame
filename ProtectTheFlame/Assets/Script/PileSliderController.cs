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
        if (PileGage <= 0f) goOutPile = true;
        PileGage = Mathf.Clamp(PileGage, 0f, 1000);
        
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
