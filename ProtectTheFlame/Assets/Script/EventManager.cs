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
        if (occurWhat <= 50 && TimeController.hour < 4)             //50%�� Ȯ���� �̺�Ʈ ����. ���� 4�� ����.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetFirewood");
        }
                else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //���� 4�� ���Ĵ� 20% Ȯ���� �̺�Ʈ ����.
                {
                    dialogueRunner.Stop();
                    //dialogueRunner.StartDialogue("GetPaper");
                }
                else if (occurWhat > 20 && occurWhat <= 50 && TimeController.hour >= 4)                 //30%�� Ȯ���� �Ȱ�.
                {
                    dialogueRunner.Stop();
                    //dialogueRunner.StartDialogue("GetPaper");
                }
        else if (occurWhat > 50 && occurWhat <= 80)                 //30%�� Ȯ���� ���� �ٶ�.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetPaper");
        }
        else if (occurWhat > 80 && occurWhat <= 95)                 //15%�� Ȯ���� ��.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetOil");
        }
        else if (occurWhat > 95 && occurWhat <= 100)                 //5%�� Ȯ���� ������. -> ���� ���ӿ��� �������� ��ư�.
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("GetTrash");
        }
    }
}
