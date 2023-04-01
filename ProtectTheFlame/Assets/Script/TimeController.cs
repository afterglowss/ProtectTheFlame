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
    private DialogueRunner dialogueRunner;
    private InMemoryVariableStorage variableStorage;

    int b;

    public static int hour = 0;
    public static int min = 0;
    public static float sec = 0f;

    public static TimeController instance;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        b = 0;
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }

    void Update()
    {
        ClockTime();
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
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("NormalEnding");
    }
    public void Remove()
    {
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("ToInfering");
    }

    [YarnCommand("timeSet")]
    public static void TimeSet()
    {
        hour = 0;
        min = 0;
        sec = 0f;
    }
}

