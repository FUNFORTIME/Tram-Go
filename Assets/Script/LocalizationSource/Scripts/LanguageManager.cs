using LanguageLocalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// use SetLanguage(int); to set language,default is 0.
    /// </summary>
    [SerializeField] private List<Localization_SOURCE> languageOb;

    private int languageIndex;

    void Start()
    {
        LoadLanguage();
    }

    
    void Update()
    {
        //Debug.Log(languageIndex);
    }

    public void SetLanguageOb(int type)
    {
        foreach (Localization_SOURCE langSource in languageOb)
        {
            langSource.LoadLanguage(type);
            languageIndex = type;
        }
    }

    public void RefreshLanguageBox()
    {
        
        foreach (Localization_SOURCE langSource in languageOb)
        {
            langSource.RefreshTextElementsAndKeys();
        }
    }

    public void LoadLanguage()
    {
        languageIndex = (int)SaveManager.instance.gameData.language;

        SetLanguageOb(languageIndex);
        RefreshLanguageBox();
    }

}
