using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public ParticleSystem snowParticleSystem;
    public ParticleSystem blizzardParticleSystem;

    float eventCoolTime;
    float eventContinueTime;

    public DialogueRunner dialogueRunner1;
    public InMemoryVariableStorage variableStorage1;

    public DialogueRunner dialogueRunner2;                      //dialogueRunner2는 대사가 20초 뒤에 사라지도록 세팅
    public InMemoryVariableStorage variableStorage2;


    public GameObject fogImage;
    public GameObject blackImage;

    float eventTime;

    int blizzardOneTime;

    public void Awake()
    {
        
    }
    public void Start()
    {
        if (DataManager.GetDifficulty() == 0) eventTime = 70f;
        else if (DataManager.GetDifficulty() == 1) eventTime = 60f;
        else eventTime = 50f;

        blizzardOneTime = 0;

        eventCoolTime = 0;
        eventContinueTime = 0;
    }

    public void Update()
    {
        if (TemparatureSliderController.TemparatureGage <= 0f) return;

        eventCoolTime += Time.deltaTime;

        if (TimeController.hour >= 5 && blizzardOneTime == 0 && eventCoolTime > 48f && eventCoolTime < 49f)
        {
            eventCoolTime = 0;
            Blizzard();
        }
        
        if (eventCoolTime > eventTime)
        {
            eventCoolTime = 0;
            if (DataManager.GetDifficulty() == 0) WhatEventIsIt_Windy();
            else if (DataManager.GetDifficulty() == 1) WhatEventIsIt_Snowy();
            else WhatEventIsIt_Blizzard();
        }

    }

    
    public void WhatEventIsIt_Windy()
    {
        Debug.Log("Windy Event");
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 40 && TimeController.hour < 4)             //50%의 확률로 이벤트 없음. 새벽 4시 이전.
        {
            NoEvent();
        }
        else if (occurWhat <= 20 && TimeController.hour >= 4)                                   //새벽 4시 이후는 20% 확률로 이벤트 없음.
        {
            NoEvent();
        }
        else if (occurWhat > 10 && occurWhat <= 40 && TimeController.hour >= 4)                 //30%의 확률로 안개.
        {
            Fog();
        }
        else if (occurWhat > 40 && occurWhat <= 70)                 //30%의 확률로 강한 바람.
        {
            StrongWind();
        }
        else if (occurWhat > 70 && occurWhat <= 95)                 //3시 전까지는 30%의 확률로 눈. 3시 이후 25% 확률
        {
            Snow();
        }
        else if (occurWhat > 95 && occurWhat <= 100 && TimeController.hour < 3)
        {
            Snow();
        }
        else if (occurWhat > 95 && occurWhat <= 100 && TimeController.hour >= 3)        //3시 이후 5%의 확률로 눈보라. -> 거의 게임오버 수준으로 어렵게.
        {
            Blizzard();
        }
    }
    public void WhatEventIsIt_Snowy()
    {
        Debug.Log("Snowy Event");
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 40 && TimeController.hour < 4)             //40%의 확률로 이벤트 없음. 새벽 4시 이전.
        {
            NoEvent();
        }
        else if (occurWhat <= 10 && TimeController.hour >= 4)                                   //새벽 4시 이후는 10% 확률로 이벤트 없음.
        {
            NoEvent();
        }
        else if (occurWhat > 10 && occurWhat <= 40 && TimeController.hour >= 4)                 //30%의 확률로 안개.
        {
            Fog();
        }
        else if (occurWhat > 40 && occurWhat <= 70)                 //30%의 확률로 강한 바람.
        {
            StrongWind();
        }
        else if (occurWhat > 70 && occurWhat <= 93)                 //2시 전까지는 30%의 확률로 눈. 2시 이후 23% 확률
        {
            Snow();
        }
        else if (occurWhat > 93 && occurWhat <= 100 && TimeController.hour < 2)
        {
            Snow();
        }
        else if (occurWhat > 93 && occurWhat <= 100 && TimeController.hour >= 2)        //2시 이후 7%의 확률로 눈보라. -> 거의 게임오버 수준으로 어렵게.
        {
            Blizzard();
        }
    }
    public void WhatEventIsIt_Blizzard()
    {
        Debug.Log("Blizzard Event");
        int occurWhat = Random.Range(1, 101);
        if (occurWhat <= 30 && TimeController.hour < 4)             //30%의 확률로 이벤트 없음. 새벽 4시 이전.
        {
            NoEvent();
        }
        else if (occurWhat <= 5 && TimeController.hour >= 4)                                   //새벽 4시 이후는 5% 확률로 이벤트 없음.
        {
            NoEvent();
        }
        else if (occurWhat > 5 && occurWhat <= 30 && TimeController.hour >= 4)                 //25%의 확률로 안개.
        {
            Fog();
        }
        else if (occurWhat > 30 && occurWhat <= 65)                 //40%의 확률로 강한 바람.
        {
            StrongWind();
        }
        else if (occurWhat > 65 && occurWhat <= 90)                 //2시 전까지는 35%의 확률로 눈. 2시 이후 25% 확률
        {
            Snow();
        }
        else if (occurWhat > 90 && occurWhat <= 100 && TimeController.hour < 2)
        {
            Snow();
        }
        else if (occurWhat > 90 && occurWhat <= 100 && TimeController.hour >= 2)        //2시 이후 10%의 확률로 눈보라. -> 거의 게임오버 수준으로 어렵게.
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
        PlayerController.instance.moveSpeed = PlayerController.lowMoveSpeed;         //플레이어 속도 느리게
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
                FlameSliderController.FlameGage -= Time.deltaTime * 25; //불꽃 게이지 1초마다 -20
            }
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 1;  //체온 게이지 1초마다 -1

            eventContinueTime += Time.deltaTime;

            if (eventContinueTime > 20f)    //20초 후에
            {
                eventContinueTime = 0;
                Debug.Log("stopStrongWind");
                PlayerController.instance.moveSpeed = PlayerController.originMoveSpeed;     //플레이어 속도 원상복구
                PlayerController.blockFanning = false;              //부채질 가능
                yield break;
            }
            yield return null;
        }
    }
    public void Snow()
    {
        dialogueRunner2.StartDialogue("Snow");
        snowParticleSystem.Play();

        StartCoroutine(Co_Snow());
    }

    public IEnumerator Co_Snow()
    {
        while (true)
        {
            PileSliderController.PileGage -= Time.deltaTime * 20;
            TemparatureSliderController.TemparatureGage -= Time.deltaTime * 3;

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
        blizzardOneTime++;
        dialogueRunner2.StartDialogue("Blizzard");
        blizzardParticleSystem.Play();

        PlayerController.instance.moveSpeed = PlayerController.lowMoveSpeed;
        PlayerController.blockFanning = true;           //부채질 불가능
        PlayerController.blockScreen = true;            //스크린 설치 불가능

        if (PlayerController.isScreen == true)
        //눈보라일 때 스크린이 설치되어있으면
        {
            CancelInvoke();                             //인보크 취소하고
            PlayerController.isScreen = false;
            PlayerController.instance.RemoveScreen();            //바로 스크린 삭제
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("DestroyScreen");
        }

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
                PlayerController.blockScreen = false;

                yield break;
            }
            yield return null;
        }
    }

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
        StartCoroutine(FadeFromTo(fogImage, 0.2f, 0.98f));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeFromTo(fogImage, 0.98f, 0.7f));
        yield return new WaitForSeconds(4f);
        StartCoroutine(FadeFromTo(fogImage, 0.7f, 1.0f));
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeFromTo(fogImage, 1.0f, 0f));
    }

    public static IEnumerator FadeFromTo(GameObject obj, float from, float to)
    {
        Debug.Log("Fadefromto 진입");
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
