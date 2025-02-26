using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private Toggle chinese;
    [SerializeField] private Toggle english;
    [SerializeField] private Toggle japanese;

    private void Start()
    {
        string path = "Assets/Texts/default.txt";
        string content = File.ReadAllText(path);
        int languageIndex;
        int.TryParse(content, out languageIndex);
        switch (languageIndex)
        {
            case 0: chinese.isOn = true; break;
            case 1: english.isOn = true; break;
            case 2: japanese.isOn = true; break;
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
