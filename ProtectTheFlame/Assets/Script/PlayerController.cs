using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private InMemoryVariableStorage variableStorage;

    public static PlayerController instance;

    public static FlameSliderController fsc;
    public static PileSliderController psc;
    public static TemparatureSliderController tsc;
    
    Rigidbody2D rigid;
    Animator anim;
    AudioSource audioSource;

    public static bool stopMove = false;

    public float moveSpeed = 3.0f;
    Vector2 move = new Vector2();

    bool inFlame = false;
    bool inPile = false;
    bool inJunk = false;
    bool inTable = false;
    bool inTent = false;

    static float time = 0;

    static int firewoodCnt = 0;
    static int paperCnt = 0;
    static int oilCnt = 0;
    static int screenCnt = 0;

    public TextMeshProUGUI firewoodTxt;
    public TextMeshProUGUI paperTxt;
    public TextMeshProUGUI oilTxt;
    public TextMeshProUGUI screenTxt;

    private static GameObject screenObj;

    bool hungry = false;


    [YarnCommand("stopMoveBool")]
    public static void StopMoveBool(bool state)     //얀 스크립트에서 stopMove bool 제어 가능
    {
        stopMove = state;
    }


    [YarnFunction("getCnt")]
    public static int GetCnt(string name)
    {
        if (name == "firewood")
        {
            return firewoodCnt;
        }
        else if (name == "paper")
        {
            return paperCnt;
        }
        else if (name == "oil")
        {
            return oilCnt;
        }
        else if (name == "screen")
        {
            return screenCnt;
        }
        else
        {
            return 0;
        }
    }

    [YarnCommand("plusCnt")]
    public static void PlusCnt(string name)
    {
        if (name == "firewood")
        {
            firewoodCnt++;
        }
        else if (name == "paper")
        {
            paperCnt++;
        }
        else if (name == "oil")
        {
            oilCnt++;
        }
        else if (name == "screen")
        {
            screenCnt++;
        }
    }
    [YarnCommand("minusCnt")]
    public static void MinusCnt(string name)
    {
        if (name == "firewood")
        {
            UseFirewood();
        }
        else if (name == "paper")
        {
            UsePaper();
        }
        else if (name == "oil")
        {
            UseOil();
        }
        else if (name == "screen")
        {
            UseScreen();
        }
    }

    public void UpdateCnt()
    {
        firewoodTxt.text = firewoodCnt.ToString();
        paperTxt.text = paperCnt.ToString();
        oilTxt.text = oilCnt.ToString();
        screenTxt.text = screenCnt.ToString();
    }

    public void Awake()
    {
        audioSource = GameObject.Find("SoundManager").GetComponentInChildren<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dialogueRunner = FindObjectOfType<DialogueRunner>();
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
        
        tsc = FindObjectOfType<TemparatureSliderController>();
        fsc = FindObjectOfType<FlameSliderController>();
        psc = FindObjectOfType<PileSliderController>();

        screenObj = GameObject.Find("GameObject").transform.Find("Screen").gameObject;
    }

    public void Update()
    {
        UpdateState();
        UpdateCnt();
        if (inFlame == true)
        {
            isFanning();
            CheckingItemUsing();
        }

        if (inPile == true)
        {
            isChopping();
        }

        if (inJunk == true)
        {
            isFinding();
        }

        if (inTable == true)
        {
            isMaking();
        }

        if (inTent == true && hungry == true)
        {
            isTenting();
        }
    }

    public void CheckingItemUsing()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1))
        {
            //stopMove = true;
            //StopPlayer();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("CheckUseFirewood");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            //stopMove = true;
            //StopPlayer();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("CheckUsePaper");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            //stopMove = true;
            //StopPlayer();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("CheckUseOil");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            //stopMove = true;
            //StopPlayer();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("CheckUseScreen");
        }
    }
    
    public void isFanning()                         //부채질 함수
    {
        if (GameManager.isPause == true) return;
        if (Input.GetKeyDown(KeyCode.Space))        //space 누른 순간 한번만 실행
        {
            dialogueRunner.StartDialogue("Fanning");
            //dialogueRunner.Stop();
        }
        else if (Input.GetKey(KeyCode.Space))            //space 누르고 있는 동안 실행
        {
            anim.SetBool("isFanning", true);
            stopMove = true;
            StopPlayer();

            fsc.FlameGage += Time.deltaTime * 40;
        }
        else if (Input.GetKeyUp(KeyCode.Space))          //space 뗀 순간 한번만 실행
        {
            //dialogueRunner.Stop();
            anim.SetBool("isFanning", false);
            stopMove = false;
        }
    }
    public void isChopping()                        //장작 패기 함수
    {
        if (GameManager.isPause == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner.StartDialogue("Chopping");
            //dialogueRunner.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isChopping", true);
            stopMove = true;
            StopPlayer();

            time += Time.deltaTime;

            if (time > 3f)
            {
                time = 0;
                GetFirewood();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isChopping", false);
            stopMove = false;
            time = 0;
        }
    }
    public void isMaking()                        //가림막 제작 함수
    {
        if (GameManager.isPause == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner.StartDialogue("Making");
            //dialogueRunner.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isMaking", true);
            stopMove = true;
            StopPlayer();

            time += Time.deltaTime;

            if (time > 5f)                      //가림막 제작에 5초 필요
            {
                time = 0;
                GetScreen();                    //가림막 획득
                //dialogueRunner.StartDialogue("GetScreen");  //가림막 획득했다는 대사 출력
                //dialogueRunner.Stop();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isMaking", false);
            stopMove = false;
            time = 0;
        }
    }
    public void isFinding()                        //잡동사니 뒤지는 함수
    {
        if (GameManager.isPause == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner.StartDialogue("Finding");
            //dialogueRunner.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isFinding", true);
            stopMove = true;
            StopPlayer();

            time += Time.deltaTime;

            if (time > 5f)                      //잡동사니 뒤지는 데 5초 필요
            {
                time = 0;
                GetItem();                    //랜덤으로 아이템 획득
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isFinding", false);
            stopMove = false;
            time = 0;
        }
    }

    public void isTenting()
    {
        if (GameManager.isPause == true) return;
    }
    public void GetFirewood()
    {
        firewoodCnt++;
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("GetFirewood");
    }

    public void GetScreen()
    {
        screenCnt++;
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("GetScreen");
    }

    public void GetItem()
    {
        int getWhat = Random.Range(1,101);
        if (getWhat <= 20)                                      //20%의 확률로 장작 획득
        {
            firewoodCnt++;
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("GetFirewood");
        }
        else if (getWhat > 20 && getWhat <= 60)                 //40%의 확률로 신문지 획득
        {
            paperCnt++;
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("GetPaper");
        }
        else if (getWhat > 60 && getWhat <= 70)                 //10%의 확률로 기름 획득
        {
            oilCnt++;
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("GetOil");
        }
        else if (getWhat > 70 && getWhat <= 95)                 //25%의 확률로 쓰레기 획득
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("GetTrash");
        }
        else                                                    //5%의 확률로 ??? 획득
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("GetUnknown");
        }
    }

    public static void UseFirewood()    //장작 사용
    {
        firewoodCnt--;
        psc.PileGage += 200;            //장작 게이지 +200
        fsc.FlameGage += 20;            //불꽃 게이지 +20
    }

    public static void UsePaper()       //신문지 사용
    {
        paperCnt--;
        fsc.FlameGage += 50;            //불꽃 게이지 +50
    }

    public static void UseOil()         //기름 사용
    {
        oilCnt--;
        fsc.FlameGage += 100;           //불꽃 게이지 +100
    }

    public static void UseScreen()      //가림막 사용
    {
        screenCnt--;
        fsc.StopFlameGage = true;       //가림막은 10초 동안 불꽃 게이지 감소를 막아줌
        screenObj.SetActive(true);      //가림막 생성

        instance.Invoke("RemoveScreen", 10f);   //10초 뒤 가림막 제거 함수 호출
    }

    private static void RemoveScreen()
    {
        screenObj.SetActive(false);
    }

    //public static IEnumerator waitTime(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //}


    private void FixedUpdate()
    {
        if (!stopMove)
            MovePlayer();
    }
    [YarnCommand("movePlayer")]
    public void MovePlayer()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
        rigid.velocity = move * moveSpeed;
    }
    [YarnCommand("stopPlayer")]
    public void StopPlayer()
    {
        move.x = 0;
        move.y = 0;
        move.Normalize();
        rigid.velocity = Vector2.zero;
    }

    private void UpdateState()
    {
        if (Mathf.Approximately(move.x, 0) && Mathf.Approximately(move.y, 0))
        {
            audioSource.Stop();
            anim.SetBool("isMove", false);
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.pitch = 1.7f;
            }
            anim.SetBool("isMove", true);
        }
        anim.SetFloat("inputx", move.x);
        anim.SetFloat("inputy", move.y);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            tsc.stopTemparatureGage = true;
            inFlame = true;
        }
        else if (other.gameObject.CompareTag("Pile"))
        {
            inPile = true;
        }
        else if (other.gameObject.CompareTag("Junk"))
        {
            inJunk = true;
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            inTable = true;
        }
        else if (other.gameObject.CompareTag("Tent"))
        {
            inTent = true;
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            
        }
        else if (other.gameObject.CompareTag("Pile"))
        {

        }
        else if (other.gameObject.CompareTag("Junk"))
        {

        }
        else if (other.gameObject.CompareTag("Table"))
        {

        }
        else if (other.gameObject.CompareTag("Tent"))
        {

        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            tsc.stopTemparatureGage = false;
            inFlame = false;
        }
        else if (other.gameObject.CompareTag("Pile"))
        {
            inPile = false;
        }
        else if (other.gameObject.CompareTag("Junk"))
        {
            inJunk = false;
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            inTable = false;
        }
        else if (other.gameObject.CompareTag("Tent"))
        {
            inTent = false;
        }
    }
    
}