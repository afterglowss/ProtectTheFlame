using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
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
        if (TemparatureSliderController.TemparatureGage <= 0f) return;
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
            PlayerController.instance.RemoveScreen();            //�ٷ� ��ũ�� ����
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("DestroyScreen");
        }
    }

    public void WhatEventIsIt()
    {
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 100 && TimeController.hour < 4)             //50%�� Ȯ���� �̺�Ʈ ����. ���� 4�� ����.
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
        else if (occurWhat > 80 && occurWhat <= 97)                 //17%�� Ȯ���� ��.
        {
            Snow();
        }
        else if (occurWhat > 96 && occurWhat <= 100)                //3%�� Ȯ���� ������. -> ���� ���ӿ��� �������� ��ư�.
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
        PlayerController.instance.moveSpeed = PlayerController.lowMoveSpeed;         //�÷��̾� �ӵ� ������
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
                FlameSliderController.FlameGage -= Time.deltaTime * 20; //�Ҳ� ������ 1�ʸ��� -20
            }
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 1;  //ü�� ������ 1�ʸ��� -1

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)    //20�� �Ŀ�
            {
                eventContinueTime = 0;
                Debug.Log("stopStrongWind");
                PlayerController.instance.moveSpeed = PlayerController.originMoveSpeed;     //�÷��̾� �ӵ� ���󺹱�
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
            PileSliderController.PileGage -= Time.deltaTime * 20;
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
        dialogueRunner2.StartDialogue("Blizzard");
        PlayerController.instance.moveSpeed = PlayerController.lowMoveSpeed;
        PlayerController.blockFanning = true;           //��ä�� �Ұ���
        PlayerController.blockScreen = true;            //��ũ�� ��ġ �Ұ���
        //PlayerController.RemoveScreen();                //��ũ�� �ı�

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
                Debug.Log("stopBlizzard");
                PlayerController.instance.moveSpeed = PlayerController.originMoveSpeed;
                PlayerController.blockFanning = false;
                PlayerController.blockScreen = true;

                yield break;
            }
            yield return null;
        }
    }
    public GameObject fogImage;

    public void Fog()
    {
        dialogueRunner2.StartDialogue("Fog");
        StartCoroutine(Co_Fog());
    }

    public IEnumerator Co_Fog()
    {
        StartCoroutine(FadeFromTo(fogImage, 0, 0.6f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeFromTo(fogImage, 0.6f, 0.2f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeFromTo(fogImage, 0.2f, 0.9f));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeFromTo(fogImage, 0.9f, 0.4f));
        yield return new WaitForSeconds(4f);
        StartCoroutine(FadeFromTo(fogImage, 0.4f, 1.0f));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeFromTo(fogImage, 1.0f, 0f));
    }

    public static IEnumerator FadeFromTo(GameObject obj, float from, float to)
    {
        Debug.Log("Fadefromto ����");
        float FadeCount;
        if (to > from)
        {
            Debug.Log("to>from");
            FadeCount = from;
            Color color;
            color = obj.GetComponent<Image>().color;
            while (FadeCount < to)
            {
                FadeCount += 0.005f;
                yield return new WaitForSeconds(0.005f);
                color.a = FadeCount;
                obj.GetComponent<Image>().color = color;
            }
        }
        else if (from > to)
        {
            Debug.Log("to<from");
            FadeCount = from;
            Color color;
            color = obj.GetComponent<Image>().color;
            while (FadeCount > to)
            {
                FadeCount -= 0.005f;
                yield return new WaitForSeconds(0.005f);
                color.a = FadeCount;
                obj.GetComponent<Image>().color = color;
            }
        }
    }
}
