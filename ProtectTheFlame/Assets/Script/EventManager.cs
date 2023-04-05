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

    public DialogueRunner dialogueRunner2;                      //dialogueRunner2�� ��簡 20�� �ڿ� ��������� ����
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

        if (PlayerController.blockScreen == true && IsInvoking() == true)   
            //���� ��ũ�� ��ġ �Ұ� �����϶� ��ũ���� ��ġ�Ǿ��ִ� isInvoking�� ���̸�
        {
            CancelInvoke();                             //�κ�ũ ����ϰ�
            PlayerController.instance.RemoveScreen();   //�ٷ� ��ũ�� ����
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("DestroyScreen");
        }
    }

    public void WhatEventIsIt()
    {
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 0 && TimeController.hour < 4)             //50%�� Ȯ���� �̺�Ʈ ����. ���� 4�� ����.
        {
            NoEvent();
        }
                else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //���� 4�� ���Ĵ� 20% Ȯ���� �̺�Ʈ ����.
                {
                    NoEvent();
                }
                else if (occurWhat > 20 && occurWhat <= 50 && TimeController.hour >= 4)                 //30%�� Ȯ���� �Ȱ�.
                {
                    Fog();
                }
        else if (occurWhat > 0 && occurWhat <= 80)                 //30%�� Ȯ���� ���� �ٶ�.
        {
            StrongWind();
        }
        else if (occurWhat > 80 && occurWhat <= 95)                 //15%�� Ȯ���� ��.
        {
            Snow();
        }
        else if (occurWhat > 95 && occurWhat <= 100)                //5%�� Ȯ���� ������. -> ���� ���ӿ��� �������� ��ư�.
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
        PlayerController.instance.moveSpeed = 2.0f;         //�÷��̾� �ӵ� ������
        PlayerController.blockFanning = true;                   //��ä�� �Ұ�

        PlayerController.instance.anim.SetBool("isFanning", false);
        PlayerController.stopMove = false;

        StartCoroutine(Co_StrongWind());                        //20�� ���� �̺�Ʈ ����
    }
    public IEnumerator Co_StrongWind()
    {
        while (true)
        {
            if (!FlameSliderController.stopFlameGage)           //���������� ���� ���� ������
            {
                FlameSliderController.FlameGage -= Time.deltaTime * 30; //�Ҳ� ������ 1�ʸ��� -30
            }
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 1;  //ü�� ������ 1�ʸ��� -1

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)    //20�� �Ŀ�
            {
                eventContinueTime = 0;
                Debug.Log("stopStrongWind");
                PlayerController.instance.moveSpeed = 3.0f;     //�÷��̾� �ӵ� ���󺹱�
                PlayerController.blockFanning = false;              //��ä�� ����
                yield break;
            }
            yield return null;
        }
    }
    public void Snow()
    {
        dialogueRunner2.StartDialogue("Snow");

        StartCoroutine(Co_Snow());
    }

    public IEnumerator Co_Snow()
    {
        while (true)
        {
            PileSliderController.PileGage -= Time.deltaTime * 30;
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 2;

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)
            {
                eventContinueTime = 0;
                Debug.Log("stopSnow");
                yield break;
            }
            yield return null;
        }
    }

    public void Blizzard()
    {
        dialogueRunner2.StartDialogue("Snow");
        PlayerController.instance.moveSpeed = 2.0f;
        PlayerController.blockFanning = true;           //��ä�� �Ұ���
        PlayerController.blockScreen = true;            //��ũ�� ��ġ �Ұ���

        PlayerController.instance.anim.SetBool("isFanning", false);
        PlayerController.stopMove = false;

        StartCoroutine(Co_Blizzard());
    }

    public IEnumerator Co_Blizzard()
    {
        while (true)
        {
            FlameSliderController.FlameGage -= Time.deltaTime * 30;
            PileSliderController.PileGage -= Time.deltaTime * 10;
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 2;

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)
            {
                eventContinueTime = 0;
                Debug.Log("stopSnow");
                PlayerController.instance.moveSpeed = 3.0f;
                PlayerController.blockFanning = false;

                yield break;
            }
            yield return null;
        }
    }

    public void Fog()
    {

    }
}
