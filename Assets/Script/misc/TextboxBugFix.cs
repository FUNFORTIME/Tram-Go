using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextboxBugFix : MonoBehaviour
{
    private string text;
    private TextMeshPro textbox;

    void Start()
    {
        textbox=GetComponent<TextMeshPro>();
        text = textbox.text;
        StartCoroutine(FixBug());
    }

    private IEnumerator FixBug()
    {
        yield return new WaitForSeconds(1);
        textbox.text = "FixBug";
        textbox.text = text;
    }
}
