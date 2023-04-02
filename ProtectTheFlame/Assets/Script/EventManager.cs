using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Yarn.Unity;

public class EventManager : MonoBehaviour
{
    float eventCoolTime = 0;
    float eventContinueTime = 0;

    public DialogueRunner dialogueRunner1;
    public InMemoryVariableStorage variableStorage1;

    public DialogueRunner dialogueRunner2;
    public InMemoryVariableStorage variableStorage2;

    private BlendTree blendTree;



    public void Awake()
    {
        //blendTree = FindObjectOfType<BlendTree>();
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
            NoEvent();
        }
                else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //���� 4�� ���Ĵ� 20% Ȯ���� �̺�Ʈ ����.
                {
                    
                }
                else if (occurWhat > 20 && occurWhat <= 50 && TimeController.hour >= 4)                 //30%�� Ȯ���� �Ȱ�.
                {
                    Fog();
                }
        else if (occurWhat > 50 && occurWhat <= 80)                 //30%�� Ȯ���� ���� �ٶ�.
        {
            StrongWind();
        }
        else if (occurWhat > 80 && occurWhat <= 95)                 //15%�� Ȯ���� ��.
        {
            Snow();
        }
        else if (occurWhat > 95 && occurWhat <= 100)                 //5%�� Ȯ���� ������. -> ���� ���ӿ��� �������� ��ư�.
        {
            Blizzard();
        }
    }

    public void NoEvent()
    {
        dialogueRunner2.StartDialogue("NoEvent");
    }

    public void StrongWind()
    {
        dialogueRunner2.StartDialogue("StrongWind");
        PlayerController.instance.moveSpeed = 2.0f;
        PlayerController.blockFanning = true;

        StartCoroutine(Co_StrongWind());
        //Invoke("StopCo_StringWind", 20f);

        //Debug.Log("stopStrongWind");
        //PlayerController.instance.moveSpeed = 3.0f;
        //PlayerController.blockFanning = false;
    }
    IEnumerator Co_StrongWind()
    {
        if (!FlameSliderController.stopFlameGage)
        {
            FlameSliderController.FlameGage -= Time.deltaTime * 30;
        }

        eventContinueTime += Time.deltaTime;

        if (eventContinueTime > 20f)
        {
            eventContinueTime = 0;
            Debug.Log("stopStrongWind");
            PlayerController.instance.moveSpeed = 3.0f;
            PlayerController.blockFanning = false;
            //dialogueRunner2.Stop();
            yield break;
        }
        yield return null;
    }
    public void StopCo_StringWind()
    {
        StopCoroutine(Co_StrongWind());
    }
    public void Snow()
    {

    }

    public void Blizzard()
    {

    }

    public void Fog()
    {

    }
}
