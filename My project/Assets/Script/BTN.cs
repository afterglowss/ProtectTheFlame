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
                StoryScene();
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
                PauseGame();
                break;
            case BTNType.PauseQuit:
                StartScene();
                ResumeGame();
                break;
            case BTNType.PauseContinue:
                CanvasGroupOff(pauseGroup);
                CanvasGroupOn(playGroup);
                ResumeGame();
                break;
            case BTNType.StorySkip:
                GameScene();
                break;
            
        }
    }
    public void StoryScene()
    {
        SceneManager.LoadScene("StoryScene");
    }
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
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

    //FlameSliderController flameSlider = GameObject.Find("FlameSlider").GetComponent<FlameSliderController>();
    //PileSliderController pileSlider = GameObject.Find("PileSlider").GetComponent<PileSliderController>();
    //TemparatureSliderController temparatureSlider = GameObject.Find("TenparatureSlider").GetComponent<TemparatureSliderController>();
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}