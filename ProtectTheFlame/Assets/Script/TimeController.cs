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

    int a;
    int b;

    public static int hour;
    public static int min;
    public static float sec;

    public static TimeController instance;

    private void Awake()
    {
        instance = this;
        hour = 0;
        min = 0;
        sec = 0f;
    }
    public void Start()
    {
        a = 0;
        b = 0;
        
    }

    void Update()
    {
        if (TemparatureSliderController.TemparatureGage <= 0f) return;
        ClockTime();
        if (hour == 4 && min == 30 && sec == 0f && a == 0)
        {
            a++;
            PlayerController.hungry = true;
            PlayerController.instance.dialogueRunner1.Stop();
            PlayerController.instance.dialogueRunner1.StartDialogue("HungryFirst");
        }
        if (hour == 6 && min == 0 && sec == 0f && b == 0)
        {
            b++;
            TimeFinish();
        }
    }
    void ClockTime()
    {
        sec += Time.deltaTime * 20; //3초가 1분으로 계산됨.
        timeText.text = String.Format("{0:D2}:{1:D2}", hour, min);
        if ((int)sec > 59)
        {
            sec = 0f;
            min++;
        }
        if (min > 59)
        {
            min = 0;
            hour++;
        }
    }

    public void TimeFinish()
    {
        GameManager.JumpScene("GameClearScene");
    }
    
    [YarnCommand("timeSet")]
    public static void TimeSet()
    {
        hour = 0;
        min = 0;
        sec = 0f;
    }
}

