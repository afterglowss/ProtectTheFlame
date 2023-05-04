using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    public DialogueRunner dialogueRunner1;
    public InMemoryVariableStorage variableStorage1;

    public DialogueRunner dialogueRunner2;
    public InMemoryVariableStorage variableStorage2;

    public static PlayerController instance;

    public static FlameSliderController fsc;
    public static PileSliderController psc;
    public static TemparatureSliderController tsc;
    
    public Rigidbody2D rigid;
    public Animator anim;
    public AudioSource audioSource;

    public static bool stopMove = false;

    public float moveSpeed;
    Vector2 move;

    public const float originMoveSpeed = 3.5f;
    public const float lowMoveSpeed = 3.0f;

    bool inFlame;
    bool inPile;
    bool inJunk;
    bool inTable;
    bool inTent;

    static float itemCoolTime;

    static int firewoodCnt;
    static int paperCnt;
    static int oilCnt;
    static int screenCnt;

    public TextMeshProUGUI firewoodTxt;
    public TextMeshProUGUI paperTxt;
    public TextMeshProUGUI oilTxt;
    public TextMeshProUGUI screenTxt;

    private static GameObject screenObj;

    bool hungry;

    public static bool isChecking;

    public static bool blockFanning;

    public static bool blockScreen;         //������ �̺�Ʈ �� ��ũ�� ��� �Ұ�. ���� �ִ� �͵� �ı���.

    public static bool isScreen;
    public void Awake()
    {
        audioSource = GameObject.Find("SoundManager").GetComponentInChildren<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //dialogueRunner1 = FindObjectOfType<DialogueRunner>();
        //variableStorage1 = FindObjectOfType<InMemoryVariableStorage>();
        
        tsc = FindObjectOfType<TemparatureSliderController>();
        fsc = FindObjectOfType<FlameSliderController>();
        psc = FindObjectOfType<PileSliderController>();

        screenObj = GameObject.Find("GameObject").transform.Find("Screen").gameObject;
        instance = this;
        
        moveSpeed = originMoveSpeed;
        move = new Vector2();

        inFlame = false;
        inPile = false;
        inJunk = false;
        inTable = false;
        inTent = false;

        itemCoolTime = 0;
        firewoodCnt = 0;
        paperCnt = 0;
        oilCnt = 0;
        screenCnt = 0;

        hungry = false;

        isChecking = false;

        blockFanning = false;

        blockScreen = false;         

        isScreen = false;
    }
    [YarnCommand("isCheckingBool")]
    public static void IsCheckingBool(bool state)
    {
        isChecking = state;
    }

    [YarnCommand("stopMoveBool")]
    public static void StopMoveBool(bool state)     //�� ��ũ��Ʈ���� stopMove bool ���� ����
    {
        stopMove = state;
    }

    [YarnFunction("getBlockScreen")]
    public static bool GetBlockScreen()
    {
        return blockScreen;
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

    

    public void Update()
    {
        UpdateState();
        UpdateCnt();
        if (inFlame == true)
        {
            IsFanning();
            CheckingItemUsing();
        }

        if (inPile == true)
        {
            IsChopping();
        }

        if (inJunk == true)
        {
            IsFinding();
        }

        if (inTable == true)
        {
            IsMaking();
        }

        if (inTent == true && hungry == true)
        {
            IsTenting();
        }
    }

    public void CheckingItemUsing()
    {
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (firewoodCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            //isChecking = true;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("CheckUseFirewood");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (paperCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            //isChecking = true;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("CheckUsePaper");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (oilCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            //isChecking = true;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("CheckUseOil");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (screenCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            if (blockScreen == true)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("BlockScreen");
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("CheckUseScreen");
        }
    }
    
    public void IsFanning()                         //��ä�� �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        
        if (Input.GetKeyDown(KeyCode.Space))        //space ���� ���� �ѹ��� ����
        {
            if (blockFanning == true || FlameSliderController.goOutFlame || PileSliderController.goOutPile)               //��ä���� �� �� ���� ��
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("BlockFanning");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Fanning");
        }
        else if (Input.GetKey(KeyCode.Space))            //space ������ �ִ� ���� ����
        {
            if (blockFanning == true || FlameSliderController.goOutFlame || PileSliderController.goOutPile)               //��ä���� �� �� ���� ��
            {
                return;
            }
            anim.SetBool("isFanning", true);
            stopMove = true;
            StopPlayer();
            
            FlameSliderController.FlameGage += Time.deltaTime * 30;
        }
        else if (Input.GetKeyUp(KeyCode.Space))          //space �� ���� �ѹ��� ����
        {
            //dialogueRunner.Stop();
            if (blockFanning == true)               //��ä���� �� �� ���� ��
            {
                anim.SetBool("isFanning", false);
                stopMove = false;
                return;
            }
            anim.SetBool("isFanning", false);
            stopMove = false;
        }
    }
    public void IsChopping()                        //���� �б� �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Chopping");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isChopping", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > 5f)
            {
                itemCoolTime = 0;
                GetFirewood();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isChopping", false);
            stopMove = false;
            itemCoolTime = 0;
        }
    }
    public void IsMaking()                        //������ ���� �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Making");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isMaking", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > 5f)                      //������ ���ۿ� 5�� �ʿ�
            {
                itemCoolTime = 0;
                GetScreen();                    //������ ȹ��
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isMaking", false);
            stopMove = false;
            itemCoolTime = 0;
        }
    }
    public void IsFinding()                        //�⵿��� ������ �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Finding");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isFinding", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > 3f)                      //�⵿��� ������ �� 5�� �ʿ�
            {
                itemCoolTime = 0;
                GetItem();                    //�������� ������ ȹ��
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("isFinding", false);
            stopMove = false;
            itemCoolTime = 0;
        }
    }

    public void IsTenting()
    {
        if (GameManager.isPause == true) return;
    }
    public void GetFirewood()
    {
        firewoodCnt++;
        dialogueRunner1.Stop();
        dialogueRunner1.StartDialogue("GetFirewood");
    }

    public void GetScreen()
    {
        screenCnt++;
        dialogueRunner1.Stop();
        dialogueRunner1.StartDialogue("GetScreen");
    }

    public void GetItem()
    {
        int getWhat = Random.Range(1,101);
        if (getWhat <= 20)                                      //20%�� Ȯ���� ���� ȹ��
        {
            firewoodCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetFirewood");
        }
        else if (getWhat > 20 && getWhat <= 60)                 //40%�� Ȯ���� �Ź��� ȹ��
        {
            paperCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetPaper");
        }
        else if (getWhat > 60 && getWhat <= 70)                 //10%�� Ȯ���� �⸧ ȹ��
        {
            oilCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetOil");
        }
        else if (getWhat > 70 && getWhat <= 95)                 //25%�� Ȯ���� ������ ȹ��
        {
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
        else                                                    //5%�� Ȯ���� ??? ȹ��
        {
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetUnknown");
        }
    }

    public static void UseFirewood()    //���� ���
    {
        firewoodCnt--;
        PileSliderController.PileGage += 200;            //���� ������ +200
        PileSliderController.goOutPile = false;
        
        if (!FlameSliderController.goOutFlame)          //�Ҳ��� �̹� �������� �ʾҴٸ�
            FlameSliderController.FlameGage += 20;            //�Ҳ� ������ +20
    }

    public static void UsePaper()       //�Ź��� ���
    {
        paperCnt--;
        if (!FlameSliderController.goOutFlame)
            FlameSliderController.FlameGage += 50;            //�Ҳ� ������ +50
    }

    public static void UseOil()         //�⸧ ���
    {
        oilCnt--;
        if (!FlameSliderController.goOutFlame)
            FlameSliderController.FlameGage += 100;           //�Ҳ� ������ +100
    }

    public static void UseScreen()      //������ ���
    {
        isScreen = true;                                    //��ũ�� ����
        screenCnt--;
        FlameSliderController.stopFlameGage = true;       //�������� 10�� ���� �Ҳ� ������ ���Ҹ� ������
        screenObj.SetActive(true);      //������ ����

        instance.Invoke("RemoveScreen", 10f);   //10�� �� ������ ���� �Լ� ȣ��
    }

    public void RemoveScreen()
    {
        isScreen = false;                                   //��ũ�� ����
        screenObj.SetActive(false);
        FlameSliderController.stopFlameGage = false;
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
            TemparatureSliderController.stopTemparatureGage = true;
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
            TemparatureSliderController.stopTemparatureGage = false;
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