using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager:MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    public GameData gameData;

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

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if(gameData == null)
            gameData = new GameData();

/*        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
*/    }

    public void SaveGame()
    {
/*        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
*/
        dataHandler.Save(gameData);
    }

    private void OnDestroy()
    {
        SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

/*    private List<ISaveManager> FindAllSaveManage()
    {
        IEnumerable<ISaveManager> saveManagers=FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
*/}
