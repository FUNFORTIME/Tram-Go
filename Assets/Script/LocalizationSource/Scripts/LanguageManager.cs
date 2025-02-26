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
    [SerializeField] private string path;

    private int languageIndex;

    void Start()
    {
        if (path == null || path == "") path = "Assets/Texts/default.txt";
        LoadLanguage();
    }

    
    void Update()
    {

    }

    public void SetLanguage(int type)
    {
        File.WriteAllText(path, $"{type}");

        if (languageOb == null)
        {
            Debug.LogError("Language list is null.");
            return;
        }

        
        if (type < 0 || type >= 3)
        {
            Debug.LogError("Invalid language type.");
            return;
        }

        
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

        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            if (int.TryParse(content, out languageIndex))
            {
                //Debug.Log("Language index loaded: " + languageIndex);
            }
            else
            {
                Debug.LogError("Failed to parse language index from file.");
            }
        }
        else
        {
            Debug.LogError("Language index file not found.");
        }
        SetLanguage(languageIndex);
        RefreshLanguageBox();
    }

}
