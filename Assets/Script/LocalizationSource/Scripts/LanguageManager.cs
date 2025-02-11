using LanguageLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    /// <summary>
    /// use SetLanguage(int); to set language,default is 0.
    /// </summary>
    [SerializeField] private Localization_SOURCE languageOb;

    void Start()
    {
        //SetLanguage(2);
    }

    
    void Update()
    {
        
    }

    public void SetLanguage(int type)
    {
        if (languageOb != null)
            languageOb.LoadLanguage(type);
    }
}
