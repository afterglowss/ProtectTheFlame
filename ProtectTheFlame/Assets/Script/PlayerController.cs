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


    public static FlameSliderController fsc;
    public static PileSliderController psc;
    public static TemparatureSliderController tsc;
    
    Rigidbody2D rigid;
    Animator anim;
    AudioSource audioSource;

    bool stopMove = false;

    public float moveSpeed = 3.0f;
    Vector2 move = new Vector2();

    bool inFlame = false;
    bool inPile = false;
    bool inJunk = false;
    bool inTable = false;
    bool inTent = false;

    float time = 0;

    static int firewoodCnt = 0;
    static int paperCnt = 0;
    static int oilCnt = 0;
    static int screenCnt = 0;

    public TextMeshProUGUI firewoodTxt;
    public TextMeshProUGUI paperTxt;
    public TextMeshProUGUI oilTxt;
    public TextMeshProUGUI screenTxt;


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
    }

    public void Update()
    {
        UpdateState();
        UpdateCnt();
        if (inFlame == true)
        {
            isFanning();
        }

        if (inPile == true)
        {
            isChopping();
        }

        if (inJunk == true)
        {

        }

        if (inTable == true)
        {
            isMaking();
        }

        if (inTent == true)
        {

        }
    }
    
    public void isFanning()                         //��ä�� �Լ�
    {
        if (Input.GetKeyDown(KeyCode.Space))        //space ���� ���� �ѹ��� ����
        {
            dialogueRunner.StartDialogue("Fanning");
            //dialogueRunner.Stop();
        }
        else if (Input.GetKey(KeyCode.Space))            //space ������ �ִ� ���� ����
        {
            anim.SetBool("isFanning", true);
            stopMove = true;
            StopPlayer();

            fsc.FlameGage += Time.deltaTime * 40;
        }
        else if (Input.GetKeyUp(KeyCode.Space))          //space �� ���� �ѹ��� ����
        {
            dialogueRunner.Stop();
            anim.SetBool("isFanning", false);
            stopMove = false;
        }
    }
    public void isChopping()                        //���� �б� �Լ�
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner.StartDialogue("Chopping");
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
            dialogueRunner.Stop();
            anim.SetBool("isChopping", false);
            stopMove = false;
            time = 0;
        }
    }
    public void isMaking()                        //������ ���� �Լ�
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner.StartDialogue("Making");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isMaking", true);
            stopMove = true;
            StopPlayer();

            time += Time.deltaTime;

            if (time > 5f)
            {
                time = 0;
                GetScreen();
                dialogueRunner.Stop();
                dialogueRunner.StartDialogue("GetScreen");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            dialogueRunner.Stop();
            anim.SetBool("isMaking", false);
            stopMove = false;
            time = 0;
        }
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
    public static void UseFirewood()    //���� ���
    {
        firewoodCnt--;
        psc.PileGage += 200;            //���� ������ +200
        fsc.FlameGage += 20;            //�Ҳ� ������ +20
    }

    public static void UsePaper()       //�Ź��� ���
    {
        paperCnt--;
        fsc.FlameGage += 50;            //�Ҳ� ������ +50
    }

    public static void UseOil()         //�⸧ ���
    {
        oilCnt--;
        fsc.FlameGage += 100;           //�Ҳ� ������ +100
    }

    public static void UseScreen()      //������ ���
    {
        screenCnt--;
        fsc.StopFlameGage = true;       //�������� 10�� ���� �Ҳ� ������ ���Ҹ� ������
        
    }

    private void FixedUpdate()
    {
        if (!stopMove)
            MovePlayer();
    }
    private void MovePlayer()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
        rigid.velocity = move * moveSpeed;
    }

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
            tsc.stopSlider = true;
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
            tsc.stopSlider = false;
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