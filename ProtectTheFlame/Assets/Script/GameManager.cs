using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private DialogueRunner dialogueRunner;
    private InMemoryVariableStorage variableStorage;
    public void Awake()
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
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        variableStorage = FindObjectOfType<InMemoryVariableStorage>();
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
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }


    [YarnCommand("jumpScene")]
    public static void JumpScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
