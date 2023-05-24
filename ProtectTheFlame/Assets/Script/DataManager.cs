using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn.Unity;

[System.Serializable]
public class Data
{
    public int tryCnt;
    public int difficulty;
    public bool isSnowyCleared;
}

public class DataManager : MonoBehaviour
{
    Data data = new Data();

    string path;

    public static DataManager instance;
    private void Awake()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        string loadJson = File.ReadAllText(path);
        data = JsonUtility.FromJson<Data>(loadJson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("plusTryCnt")]
    public static void PlusTryCnt()
    {
        instance.data.tryCnt++;
        instance.SaveData();
    }

    [YarnFunction("getTryCnt")]
    public static int GetTryCnt()
    {
        return instance.data.tryCnt;
    }
    [YarnCommand("printTryCnt")]
    public static void PrintTryCnt()
    {
        Debug.Log(instance.data.tryCnt);
    }

    [YarnFunction("getDifficulty")]
    public static int GetDifficulty()
    {
        Debug.Log(instance.data.difficulty);
        return instance.data.difficulty;
    }
    [YarnFunction("getIsSnowyCleared")]
    public static bool GetIsSnowyCleared()
    {
        return instance.data.isSnowyCleared;
    }

    [YarnCommand("setDifficulty")]
    public static void SetDifficulty(int dif)
    {
        instance.data.difficulty = dif;
        instance.SaveData();
    }
    [YarnCommand("setTrueIsSnowyCleared")]
    public static void SetTrueIsSnowyCleared()
    {
        instance.data.isSnowyCleared = true;
        instance.SaveData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
}
