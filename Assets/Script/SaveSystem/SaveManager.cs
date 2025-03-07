using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

[Serializable]
public enum LanguageType
{
    chinese,
    english,
    japanese,
}

[Serializable]
public enum GameMode
{
    normal,
    hard,
    real,
}

public class SaveManager:MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    public GameData gameData;

    public LanguageManager languageManager;
    //private List<ISaveManager> saveManagers;

    private FileDataHandler dataHandler;

    private void Start()
    {
        //saveManagers=FindAllSaveManage();

    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        SceneManager.activeSceneChanged += OnSceneChanged;
        dataHandler=new FileDataHandler(Application.persistentDataPath,fileName);
        LoadGame();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if(gameData == null)
            gameData = new GameData();

        //gameData.CompleteCheck();
        //foreach (ISaveManager saveManager in saveManagers)
        //{
        //    saveManager.LoadData(gameData);
        //}
    }

    public void SaveGame()
    {
        //foreach (ISaveManager saveManager in saveManagers)
        //{
        //    saveManager.SaveData(ref gameData);
        //}

        dataHandler.Save(gameData);
    }

    public void ResetGame()
    {
        gameData = new GameData();
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnSceneChanged(Scene current,Scene next)
    {
        SaveGame();
    }

    public void SetLanguageType(int language)
    {
        gameData.language = (LanguageType)language;
        languageManager.SetLanguageOb(language);
        SaveGame();
    }
    public void SetGameMode(int gameMode)
    {
        gameData.gameMode = (GameMode)gameMode;
        SaveGame();
    }

    //private List<ISaveManager> FindAllSaveManage()
    //{
    //    IEnumerable<ISaveManager> saveManagers=FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

    //    return new List<ISaveManager>(saveManagers);
    //}
}
