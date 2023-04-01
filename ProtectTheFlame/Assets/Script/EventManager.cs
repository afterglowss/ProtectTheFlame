using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EventManager : MonoBehaviour
{
    float eventCoolTime = 0;
    float eventContinueTime = 0;

    private DialogueRunner dialogueRunner;
    private InMemoryVariableStorage variableStorage;

    public void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
    }

    public void Update()
    {
        eventCoolTime += Time.deltaTime;
        
        if (eventCoolTime > 60f)
        {
            eventCoolTime = 0;
            WhatEventIsIt();
        }
    }

    public void WhatEventIsIt()
    {
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 50 && TimeController.hour < 4)             //50%의 확률로 이벤트 없음. 새벽 4시 이전.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetFirewood");
        }
                else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //새벽 4시 이후는 20% 확률로 이벤트 없음.
                {
                    dialogueRunner.Stop();
                    //dialogueRunner.StartDialogue("GetPaper");
                }
                else if (occurWhat > 20 && occurWhat <= 50 && TimeController.hour >= 4)                 //30%의 확률로 안개.
                {
                    dialogueRunner.Stop();
                    //dialogueRunner.StartDialogue("GetPaper");
                }
        else if (occurWhat > 50 && occurWhat <= 80)                 //30%의 확률로 강한 바람.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetPaper");
        }
        else if (occurWhat > 80 && occurWhat <= 95)                 //15%의 확률로 눈.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetOil");
        }
        else if (occurWhat > 95 && occurWhat <= 100)                 //5%의 확률로 눈보라. -> 거의 게임오버 수준으로 어렵게.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetTrash");
        }
    }
}
