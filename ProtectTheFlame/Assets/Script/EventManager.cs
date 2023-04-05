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

    public DialogueRunner dialogueRunner2;                      //dialogueRunner2는 대사가 20초 뒤에 사라지도록 세팅
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
            //만약 스크린 설치 불가 상태일때 스크린이 설치되어있는 isInvoking이 참이면
        {
            CancelInvoke();                             //인보크 취소하고
            PlayerController.instance.RemoveScreen();   //바로 스크린 삭제
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("DestroyScreen");
        }
    }

    public void WhatEventIsIt()
    {
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 0 && TimeController.hour < 4)             //50%의 확률로 이벤트 없음. 새벽 4시 이전.
        {
            NoEvent();
        }
                else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //새벽 4시 이후는 20% 확률로 이벤트 없음.
                {
                    NoEvent();
                }
                else if (occurWhat > 20 && occurWhat <= 50 && TimeController.hour >= 4)                 //30%의 확률로 안개.
                {
                    Fog();
                }
        else if (occurWhat > 0 && occurWhat <= 80)                 //30%의 확률로 강한 바람.
        {
            StrongWind();
        }
        else if (occurWhat > 80 && occurWhat <= 95)                 //15%의 확률로 눈.
        {
            Snow();
        }
        else if (occurWhat > 95 && occurWhat <= 100)                //5%의 확률로 눈보라. -> 거의 게임오버 수준으로 어렵게.
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
        PlayerController.instance.moveSpeed = 2.0f;         //플레이어 속도 느리게
        PlayerController.blockFanning = true;                   //부채질 불가

        PlayerController.instance.anim.SetBool("isFanning", false);
        PlayerController.stopMove = false;

        StartCoroutine(Co_StrongWind());                        //20초 동안 이벤트 실행
    }
    public IEnumerator Co_StrongWind()
    {
        while (true)
        {
            if (!FlameSliderController.stopFlameGage)           //가림막으로 막혀 있지 않으면
            {
                FlameSliderController.FlameGage -= Time.deltaTime * 30; //불꽃 게이지 1초마다 -30
            }
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 1;  //체온 게이지 1초마다 -1

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)    //20초 후에
            {
                eventContinueTime = 0;
                Debug.Log("stopStrongWind");
                PlayerController.instance.moveSpeed = 3.0f;     //플레이어 속도 원상복구
                PlayerController.blockFanning = false;              //부채질 가능
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
        PlayerController.blockFanning = true;           //부채질 불가능
        PlayerController.blockScreen = true;            //스크린 설치 불가능

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
