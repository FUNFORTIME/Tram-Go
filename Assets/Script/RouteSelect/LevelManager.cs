using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameData gameData;

    public static LevelManager instance;
    [SerializeField] private GameObject levelDisplayPrefab;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void Show(Route route)
    {
        gameData = SaveManager.instance.gameData;

        transform.parent.gameObject.SetActive(true);

        for (int i = 0;i<transform.childCount;i++)
            Destroy(transform.GetChild(i).gameObject);

        for (int i = 0; i < route.levelList.Count; i++)
        {
            Level level = route.levelList[i];

            GameObject _obj = Instantiate(levelDisplayPrefab);
            LevelDisplay _levelDisplay = _obj.GetComponent<LevelDisplay>();

            if (!gameData.levelHighScore.ContainsKey(level.GetString))
                gameData.levelHighScore.Add(level.GetString, 0);

            _levelDisplay.level = level;

            _levelDisplay.level.highScore = gameData.levelHighScore[level.GetString];
            _levelDisplay.UpdateDisplay();
            _obj.transform.SetParent(transform);
        }

        SaveManager.instance.gameData = gameData;
        SaveManager.instance.SaveGame();
    }

    private void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
