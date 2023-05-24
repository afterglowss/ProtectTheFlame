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
    public const float lowMoveSpeed = 2.7f;

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

    public static bool hungry;

    public static bool isChecking;

    public static bool blockFanning;

    public static bool blockScreen;         //������ �̺�Ʈ �� ��ũ�� ��� �Ұ�. ���� �ִ� �͵� �ı���.

    public static bool isScreen;

    public static bool gotUnknown;

    public GameObject Pile1Outline;
    public GameObject Pile2Outline;
    public GameObject TableOutline;
    public GameObject TentOutline;
    public GameObject JunkOutline;

    float choppingTime;
    float makingTime;
    float screenLeftTime;



    public void Awake()
    {
        //Fade.FadeOut("FogImage");

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
        
        stopMove = false;
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

        gotUnknown = false;


    }
    private void Start()
    {
        switch (DataManager.GetDifficulty())
        {

            case 1:
                choppingTime = 10f;
                makingTime = 4f;
                screenLeftTime = 13f;
                break;
            case 0:
                choppingTime = 3f;
                makingTime = 4f;
                screenLeftTime = 15f;
                break;
            case 2:
                choppingTime = 5f;
                makingTime = 5f;
                screenLeftTime = 10f;
                break;
        }
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

    [YarnCommand("minusCnt")]
    public static void MinusCnt(string name)
    {
        switch (name)
        {
            case "firewood":
                UseFirewood();
                break;
            case "paper":
                UsePaper();
                break;
            case "oil":
                UseOil();
                break;
            case "screen":
                UseScreen();
                break;
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
        if (TemparatureSliderController.TemparatureGage <= 0f) return;
        if (GameManager.isPause == true) return;
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

        if (inTent == true)
        {
            IsTenting();
        }
    }

    public void CheckingItemUsing()
    {
        if (isChecking == true) return;
        if (stopMove == true) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (firewoodCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            dialogueRunner1.Stop();
            //dialogueRunner1.StartDialogue("CheckUseFirewood");
            dialogueRunner1.StartDialogue("UseFirewood");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (paperCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            dialogueRunner1.Stop();
            //dialogueRunner1.StartDialogue("CheckUsePaper");
            dialogueRunner1.StartDialogue("UsePaper");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (oilCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            dialogueRunner1.Stop();
            //dialogueRunner1.StartDialogue("CheckUseOil");
            dialogueRunner1.StartDialogue("UseOil");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (screenCnt == 0)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("ZeroCnt");
                return;
            }
            if (isScreen == true)
            {
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("AlreadyScreen");
                return;
            }
            if (blockScreen == true)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("BlockScreen");
                return;
            }
            dialogueRunner1.Stop();
            //dialogueRunner1.StartDialogue("CheckUseScreen");
            dialogueRunner1.StartDialogue("UseScreen");
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
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("BlockFanning");
                return;
            }
            if (hungry == true)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Hungry");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Fanning");
        }
        else if (Input.GetKey(KeyCode.Space))            //space ������ �ִ� ���� ����
        {
            if (blockFanning == true || FlameSliderController.goOutFlame || PileSliderController.goOutPile || hungry == true)               //��ä���� �� �� ���� ��
            {
                return;
            }
            anim.SetBool("isFanning", true);
            stopMove = true;
            StopPlayer();
            
            FlameSliderController.FlameGage += Time.deltaTime * 30;

            if (!audioSource.isPlaying)
            {
                //SoundManager.instance.PlaySound("fanning");
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))          //space �� ���� �ѹ��� ����
        {
            anim.SetBool("isFanning", false);
            stopMove = false;
            audioSource.Stop();

            if (blockFanning == true || FlameSliderController.goOutFlame || PileSliderController.goOutPile || hungry == true)
                return; //��ä���� �� �� ���� ��
        }
    }
    public void StopsOnPause()
    {
        anim.SetBool("isFanning", false);
        anim.SetBool("isChopping", false);
        anim.SetBool("isMaking", false);
        anim.SetBool("isFinding", false);
        stopMove = false;
        itemCoolTime = 0;
        audioSource.Stop();
        isChecking = false;
    }
    public void IsChopping()                        //���� �б� �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hungry == true)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Hungry");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Chopping");
            
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (hungry == true)
            {
                return;
            }
            anim.SetBool("isChopping", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > choppingTime)
            {
                itemCoolTime = 0;
                GetFirewood();
            }
            if (!audioSource.isPlaying)
            {
                SoundManager.instance.PlaySound("chopping");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("isChopping", false);
            stopMove = false;
            itemCoolTime = 0;
            audioSource.Stop();

            if (hungry == true) return;

        }
    }
    public void IsMaking()                        //������ ���� �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hungry == true)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Hungry");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Making");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (hungry == true)
            {
                return;
            }
            anim.SetBool("isMaking", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > makingTime)                      //������ ���ۿ� 5�� �ʿ�
            {
                itemCoolTime = 0;
                GetScreen();                    //������ ȹ��
            }
            if (!audioSource.isPlaying)
            {
                SoundManager.instance.PlaySound("making");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //dialogueRunner.Stop();
            anim.SetBool("isMaking", false);
            stopMove = false;
            itemCoolTime = 0;
            audioSource.Stop();
            if (hungry == true) return;
        }
    }
    public void IsFinding()                        //�⵿��� ������ �Լ�
    {
        if (GameManager.isPause == true) return;
        if (isChecking == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hungry == true)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Hungry");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Finding");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (hungry == true)
            {
                return;
            }
            anim.SetBool("isFinding", true);
            stopMove = true;
            StopPlayer();

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime > 3f)                      //�⵿��� ������ �� 3�� �ʿ�
            {
                itemCoolTime = 0;
                if (DataManager.GetDifficulty() == 0) GetItem_Windy();
                else if (DataManager.GetDifficulty() == 1) GetItem_Snowy();
                else GetItem_Blizzard();                    //�������� ������ ȹ��
            }
            if (!audioSource.isPlaying)
            {
                SoundManager.instance.PlaySound("finding");
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("isFinding", false);
            stopMove = false;
            itemCoolTime = 0;
            audioSource.Stop();
            if (hungry == true) return;
        }
    }

    public void IsTenting()
    {
        if (GameManager.isPause == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hungry == false)
            {
                SoundManager.instance.PlaySound("block");
                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Tent");
                return;
            }
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("Eating");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (hungry == false) return;
            stopMove = true;
            StopPlayer();

            if (!audioSource.isPlaying)
            {
                SoundManager.instance.PlaySound("eating");
            }

            itemCoolTime += Time.deltaTime;

            if (itemCoolTime >= 7f)                      //��Ʈ�� �ķ� �Դµ� 7�� �ʿ�
            {
                itemCoolTime = 0;

                stopMove = false;
                hungry = false;
                audioSource.Stop();

                dialogueRunner1.Stop();
                dialogueRunner1.StartDialogue("Full");
                return;
            }
        }
    }
    public void GetFirewood()
    {
        SoundManager.instance.PlaySound("getitem");
        firewoodCnt++;
        dialogueRunner1.Stop();
        dialogueRunner1.StartDialogue("GetFirewood");
    }

    public void GetScreen()
    {
        SoundManager.instance.PlaySound("getitem");
        screenCnt++;
        dialogueRunner1.Stop();
        dialogueRunner1.StartDialogue("GetScreen");
    }
    public void GetItem_Windy()
    {
        int getWhat = Random.Range(1, 101);
        if (getWhat <= 25)                                      //25%�� Ȯ���� ���� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            firewoodCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetFirewood");
        }
        else if (getWhat > 25 && getWhat <= 60)                 //35%�� Ȯ���� �Ź��� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            paperCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetPaper");
        }
        else if (getWhat > 60 && getWhat <= 85)                 //25%�� Ȯ���� �⸧ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            oilCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetOil");
        }
        else if (getWhat > 85 && getWhat <= 99)                 //15%�� Ȯ���� ������ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
        else if (getWhat == 100 && gotUnknown == false)         //1%�� Ȯ���� �� �ѹ� ??? ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetUnknown");
            gotUnknown = true;
        }
        else if (getWhat == 100)
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
    }
    public void GetItem_Snowy()
    {
        int getWhat = Random.Range(1,101);
        if (getWhat <= 25)                                      //25%�� Ȯ���� ���� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            firewoodCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetFirewood");
        }
        else if (getWhat > 25 && getWhat <= 60)                 //35%�� Ȯ���� �Ź��� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            paperCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetPaper");
        }
        else if (getWhat > 60 && getWhat <= 80)                 //20%�� Ȯ���� �⸧ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            oilCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetOil");
        }
        else if (getWhat > 80 && getWhat <= 99)                 //20%�� Ȯ���� ������ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
        else if (getWhat == 100 && gotUnknown == false)         //1%�� Ȯ���� �� �ѹ� ??? ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetUnknown");
            gotUnknown = true;
        }
        else if (getWhat == 100)                 
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
    }
    public void GetItem_Blizzard()
    {
        int getWhat = Random.Range(1, 101);
        if (getWhat <= 25)                                      //25%�� Ȯ���� ���� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            firewoodCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetFirewood");
        }
        else if (getWhat > 25 && getWhat <= 55)                 //30%�� Ȯ���� �Ź��� ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            paperCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetPaper");
        }
        else if (getWhat > 55 && getWhat <= 70)                 //15%�� Ȯ���� �⸧ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            oilCnt++;
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetOil");
        }
        else if (getWhat > 70 && getWhat <= 99)                 //30%�� Ȯ���� ������ ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
        else if (getWhat == 100 && gotUnknown == false)         //1%�� Ȯ���� �� �ѹ� ??? ȹ��
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetUnknown");
            gotUnknown = true;
        }
        else if (getWhat == 100)
        {
            SoundManager.instance.PlaySound("getitem");
            dialogueRunner1.Stop();
            dialogueRunner1.StartDialogue("GetTrash");
        }
    }

    public static void UseFirewood()    //���� ���
    {
        firewoodCnt--;
        SoundManager.instance.PlaySound("useitem");
        PileSliderController.PileGage += 200;            //���� ������ +200
        PileSliderController.goOutPile = false;
        
        if (!FlameSliderController.goOutFlame)          //�Ҳ��� �̹� �������� �ʾҴٸ�
            FlameSliderController.FlameGage += 20;            //�Ҳ� ������ +20
    }

    public static void UsePaper()       //�Ź��� ���
    {
        paperCnt--;
        SoundManager.instance.PlaySound("useitem");
        if (!FlameSliderController.goOutFlame)
            FlameSliderController.FlameGage += 70;            //�Ҳ� ������ +70
        PileSliderController.PileGage += 50;
    }

    public static void UseOil()         //�⸧ ���
    {
        oilCnt--;
        SoundManager.instance.PlaySound("useitem");
        if (!FlameSliderController.goOutFlame)
            FlameSliderController.FlameGage += 200;           //�Ҳ� ������ +200
    }

    public static void UseScreen()      //������ ���
    {
        
        isScreen = true;                                    //��ũ�� ����
        screenCnt--;
        SoundManager.instance.PlaySound("usescreen");
        FlameSliderController.stopFlameGage = true;       //�������� 10�� ���� �Ҳ� ������ ���Ҹ� ������
        screenObj.SetActive(true);      //������ ����

        instance.Invoke("RemoveScreen", instance.screenLeftTime);   //10�� �� ������ ���� �Լ� ȣ��
    }

    public void RemoveScreen()
    {
        isScreen = false;                                   //��ũ�� ����
        screenObj.SetActive(false);
        FlameSliderController.stopFlameGage = false;
    }
    public RuntimeAnimatorController downAnimatorController;
    public void GameOver()
    {
        stopMove = true;
        StopPlayer();
        anim.runtimeAnimatorController = downAnimatorController;
    }


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
            anim.SetBool("isMove", false);
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                SoundManager.instance.PlaySound("snowstep");
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
            Fade.ObjectAppear(Pile1Outline);
            Fade.ObjectAppear(Pile2Outline);
        }
        else if (other.gameObject.CompareTag("Junk"))
        {
            inJunk = true;
            Fade.ObjectAppear(JunkOutline);
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            inTable = true;
            Fade.ObjectAppear(TableOutline);
        }
        else if (other.gameObject.CompareTag("Tent"))
        {
            inTent = true;
            Fade.ObjectAppear(TentOutline);
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
            Fade.ObjectDisappear(Pile1Outline);
            Fade.ObjectDisappear(Pile2Outline);
        }
        else if (other.gameObject.CompareTag("Junk"))
        {
            inJunk = false;
            Fade.ObjectDisappear(JunkOutline);
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            inTable = false;
            Fade.ObjectDisappear(TableOutline);
        }
        else if (other.gameObject.CompareTag("Tent"))
        {
            inTent = false;
            Fade.ObjectDisappear(TentOutline);
        }
    }
    
}