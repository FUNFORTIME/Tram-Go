using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class LevelManager : MonoBehaviour
{
    private GameData gameData;

    public static LevelManager instance;
    [SerializeField] private GameObject levelDisplayPrefab;
    [SerializeField] private GameObject levelSelectPanel;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Hide();
    }

    public void Show(Route route)
    {
        levelSelectPanel.SetActive(true);
        gameData = SaveManager.instance.gameData;


        for (int i = 0;i<transform.childCount;i++)
            Destroy(transform.GetChild(i).gameObject);

        for (int i = 0; i < route.levelList.Count; i++)
        {
            Level level = route.levelList[i];

            GameObject _obj = Instantiate(levelDisplayPrefab, transform);
            LevelDisplay _levelDisplay = _obj.GetComponent<LevelDisplay>();

            _levelDisplay.level = level;

            _levelDisplay.level.highScore = gameData.levelHighScore[level.GetString];
            _levelDisplay.level.unlock=gameData.levelUnlock[level.GetString];
            _levelDisplay.UpdateDisplay();
        }
    }

    private void Hide()
    {
        levelSelectPanel.SetActive(false);
    }
}
