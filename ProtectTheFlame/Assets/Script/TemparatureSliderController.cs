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
    int dieOneTime;

    public GameObject warningImage;
    Color color;

    private void Start()
    {
        stopTemparatureGage = false;
        TemparatureGage = 70;
        color = warningImage.GetComponent<Image>().color;
        dieOneTime = 0;
    }

    void Update()
    {
        if (TemparatureGage <= 0 && dieOneTime == 0)
        {
            SoundManager.instance.StopSound();
            PlayerController.instance.GameOver();
            PlayerController.instance.dialogueRunner1.Stop();
            PlayerController.instance.dialogueRunner1.StartDialogue("GameOver");
            dieOneTime++;
        }
        TemparatureGage = Mathf.Clamp(TemparatureGage, 0, 100);
        
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
            TemparatureGage += Time.deltaTime * 3.5f;
        }
        GetComponent<Slider>().value = (int)TemparatureGage;
    }
}
