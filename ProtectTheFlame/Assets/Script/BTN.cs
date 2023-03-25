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
    Pause,
    PauseQuit,
    PauseContinue,
    StorySkip
}

public class BTN : MonoBehaviour
{
    public BTNType currentType;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup pauseGroup;
    public CanvasGroup playGroup;
    
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.New:
                GameManager.StoryScene();
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Back:
                CanvasGroupOff(optionGroup);
                CanvasGroupOn(mainGroup);
                break;
            case BTNType.Quit:
                Application.Quit();
                Debug.Log("╬ша╬╥А");
                break;
            case BTNType.Pause:
                CanvasGroupOn(pauseGroup);
                CanvasGroupOff(playGroup);
                GameManager.PauseGame();
                break;
            case BTNType.PauseQuit:
                GameManager.StartScene();
                GameManager.ResumeGame();
                break;
            case BTNType.PauseContinue:
                CanvasGroupOff(pauseGroup);
                CanvasGroupOn(playGroup);
                GameManager.ResumeGame();
                break;
            case BTNType.StorySkip:
                GameManager.StartScene();
                break;
            
        }
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