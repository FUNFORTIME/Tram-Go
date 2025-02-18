using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpenController : MonoBehaviour
{
    [SerializeField] private GameObject LanguageManagerHolder;
    private void OnEnable()
    {
        LanguageManager languageManager = LanguageManagerHolder.GetComponent<LanguageManager>();
        languageManager.LoadLanguage();
    }

    private void OnDisable()
    {

    }
}
