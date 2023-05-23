using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using System.IO;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public static bool isPause = false;

    public Button pauseBtn;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(instance);
        }

    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (TemparatureSliderController.TemparatureGage <= 0f) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseBtn.GetComponent<BTN>().optionGroup.alpha == 0)
                {
                    PlayerController.instance.StopsOnPause();       //일시정지 시 하고 있던 행위 멈추기
                    pauseBtn.GetComponent<BTN>().OptionGroupOn();
                }
                else
                {
                    pauseBtn.GetComponent<BTN>().OptionGroupOff();
                }
            }

        }
    }


    public static void StoryScene()
    {
        SceneManager.LoadScene("StoryScene");
    }
    public static void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public static void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
        isPause = true;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
        isPause = false;
    }


    [YarnCommand("jumpScene")]
    public static void JumpScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [YarnFunction("getTryCnt")]
    public static void GetTryCnt()
    {
        
    }

}
