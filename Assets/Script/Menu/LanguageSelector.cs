using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private Toggle chinese;
    [SerializeField] private Toggle english;
    [SerializeField] private Toggle japanese;

    private void Start()
    {
        switch (SaveManager.instance.gameData.language)
        {
            case LanguageType.chinese: chinese.isOn = true; break;
            case LanguageType.english: english.isOn = true; break;
            case LanguageType.japanese: japanese.isOn = true; break;
        }
    }

    public void ChangeLanguage()
    {
        if (chinese.isOn) SaveManager.instance.gameData.language = LanguageType.chinese;
        if(english.isOn)SaveManager.instance.gameData.language= LanguageType.english;
        if (japanese.isOn) SaveManager.instance.gameData.language = LanguageType.japanese;

        SaveManager.instance.SaveGame();
    }
}
