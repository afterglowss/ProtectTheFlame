using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemparatureSliderController : MonoBehaviour
{
    [HideInInspector]
    public static float TemparatureGage;
    [HideInInspector]
    public static bool stopTemparatureGage;
    int a = 0;


    private void Start()
    {
        stopTemparatureGage = false;
        TemparatureGage = 70;
    }

    void Update()
    {
        if (TemparatureGage < 0)
        {
            TemparatureGage = 0;
            if (a == 0)
            {
                PlayerController.instance.GameOver();
                PlayerController.instance.dialogueRunner1.Stop();
                PlayerController.instance.dialogueRunner1.StartDialogue("GameOver");
                a++;
            }
        }
        if (TemparatureGage > 100)
        {
            TemparatureGage = 100;
        }
        TempGUpDown();
    }
     
    public void TempGUpDown()
    {
        if (!stopTemparatureGage || FlameSliderController.goOutFlame)
        {
            TemparatureGage -= Time.deltaTime * 2;
        }
        else if (stopTemparatureGage)
        {
            TemparatureGage += Time.deltaTime * 3;
        }
        GetComponent<Slider>().value = (int)TemparatureGage;
    }
}
