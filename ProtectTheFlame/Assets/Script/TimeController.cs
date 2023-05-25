using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class TimeController : MonoBehaviour
{
    [SerializeField]

    public TextMeshProUGUI timeText;

    public DialogueRunner dialogueRunner1;
    public InMemoryVariableStorage variableStorage1;

    public DialogueRunner dialogueRunner2;
    public InMemoryVariableStorage variableStorage2;

    int hungryOneTime;
    int clearOneTime;

    public static int hour;
    public static int min;
    public static double sec;

    public static TimeController instance;

    private void Awake()
    {
        instance = this;
        hour = 0;
        min = 0;
        sec = 0;
        hungryOneTime = 0;
        clearOneTime = 0;
    }
    public void Start()
    {
        //hungryOneTime = 0;
        //clearOneTime = 0;
        
    }

    void Update()
    {
        if (TemparatureSliderController.TemparatureGage <= 0f) return;
        ClockTime();
        if (hour == 4 && min == 5 && sec >= 0 && hungryOneTime == 0)
        {
            hungryOneTime++;
            PlayerController.hungry = true;

            PlayerController.instance.StopsOnPause();

            PlayerController.instance.dialogueRunner1.Stop();
            PlayerController.instance.dialogueRunner1.StartDialogue("HungryFirst");
        }
        if (hour == 6 && min == 0 && sec >= 0 && clearOneTime == 0 && PlayerController.gotUnknown == false)
        {
            clearOneTime++;
            //if (DataManager.GetDifficulty() == 1) DataManager.SetTrueIsSnowyCleared();
            StartCoroutine(Fade.FadeIn(EventManager.instance.blackImage));
            Invoke("NormalEnding", 1.5f);
        }
        else if (hour == 6 && min == 0 && sec >= 0 && clearOneTime == 0 && PlayerController.gotUnknown == true)
        {
            clearOneTime++;
            //if (DataManager.GetDifficulty() == 1) DataManager.SetTrueIsSnowyCleared();
            StartCoroutine(Fade.FadeIn(EventManager.instance.fogImage));
            Invoke("TrueEnding", 1.5f);
        }
    }
    void ClockTime()
    {
        sec += Time.deltaTime * 20; //3초가 1분으로 계산됨.
        timeText.text = String.Format("{0:D2}:{1:D2}", hour, min);
        if (sec >= 60)
        {
            sec -= 60;
            min++;
        }
        if (min > 59)
        {
            min = 0;
            hour++;
        }
    }

    public void NormalEnding()
    {
        GameManager.JumpScene("GameClearScene");
    }
    public void TrueEnding()
    {
        GameManager.JumpScene("TrueEndingScene");
    }

    [YarnCommand("timeSet")]
    public static void TimeSet()
    {
        hour = 0;
        min = 0;
        sec = 0;
    }
}

