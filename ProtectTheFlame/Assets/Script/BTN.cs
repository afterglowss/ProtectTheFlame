using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BTNType
{
    New,
    Option,
    Music,
    Sound,
    Back,
    Quit,
    GoMain
}

public class BTN : MonoBehaviour
{
    public BTNType currentType;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.New:
                GameManager.StoryScene();
                break;
            case BTNType.Option:
                if (SceneManager.GetActiveScene().name == "GameScene")
                {
                    if (TemparatureSliderController.TemparatureGage <= 0f) return;
                }
                OptionGroupOn();
                break;
            case BTNType.Back:
                OptionGroupOff();
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("╬ша╬╥А");
                break;
            case BTNType.GoMain:
                GameManager.ResumeGame();
                SoundManager.instance.StopSound();
                MusicManager.instance.UnPauseMusic();
                GameManager.JumpScene("StartScene");
                break;
        }
    }

    public void OptionGroupOn()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SoundManager.instance.StopSound();
            SoundManager.instance.PlaySound("pause");
        }
        CanvasGroupOn(optionGroup);
        CanvasGroupOff(mainGroup);
        GameManager.PauseGame();
        //PlayerController.StopMoveBool(true);
        //PlayerController.instance.StopPlayer();
    }
    public void OptionGroupOff()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            SoundManager.instance.StopSound();
            SoundManager.instance.PlaySound("unpause");
        }
        CanvasGroupOn(mainGroup);
        CanvasGroupOff(optionGroup);
        GameManager.ResumeGame();
        //PlayerController.StopMoveBool(false);
        //PlayerController.instance.MovePlayer();
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    
}