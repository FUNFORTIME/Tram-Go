using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour
{
    [SerializeField] private Toggle normal;
    [SerializeField] private Toggle hard;
    [SerializeField] private Toggle real;

    private void Start()
    {
        switch (SaveManager.instance.gameData.gameMode)
        {
            case GameMode.normal: normal.isOn = true; break;
            case GameMode.hard: hard.isOn = true; break;
            case GameMode.real: real.isOn = true; break;
        }
    }

    public void ChangeGameMode()
    {
        if (normal.isOn) SaveManager.instance.gameData.gameMode = GameMode.normal;
        if (hard.isOn) SaveManager.instance.gameData.gameMode = GameMode.hard;
        if(real.isOn)SaveManager.instance.gameData.gameMode=GameMode.real;

        SaveManager.instance.SaveGame();
    }
}
