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

    public GameObject warningImage;
    Color color;

    private void Start()
    {
        stopTemparatureGage = false;
        TemparatureGage = 70;
        color = warningImage.GetComponent<Image>().color;
    }

    void Update()
    {
        if (TemparatureGage < 0)
        {
            TemparatureGage = 0;
            if (a == 0)
            {
                SoundManager.instance.StopSound();
                PlayerController.instance.GameOver();
                PlayerController.instance.dialogueRunner1.Stop();
                PlayerController.instance.dialogueRunner1.StartDialogue("GameOver");
                PlayerController.instance.dialogueRunner2.Stop();
                PlayerController.instance.dialogueRunner2.StartDialogue("NoEvent");
                a++;
            }
        }
        if (TemparatureGage > 100)
        {
            TemparatureGage = 100;
        }
        //color.a = 70 - TemparatureGage;
        //warningImage.GetComponent<Image>().color = color;
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
