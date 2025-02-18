using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public GameObject textHolder;
    public bool onUpdate;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log(transform.parent.name);
        SetText();
    }

    private void SetText()
    {
        if (textMeshPro == null)
        {
            Debug.Log("TextSetter:No textMeshPro!");
            textMeshPro.text = TextHelper.GetTextFromChild(textHolder, "NULL");
            return;
        }
        if (textHolder == null)
        {
            Debug.Log("TextSetter:No textHolder!");
            textMeshPro.text = TextHelper.GetTextFromChild(textHolder, "NULL");
            return;
        }
        if (transform.parent == null)
        {
            Debug.Log("TextSetter:No parent");
            textMeshPro.text = TextHelper.GetTextFromChild(textHolder, "NULL");
            return;
        }



        if(string.IsNullOrEmpty(key)) 
        {
            textMeshPro.text = TextHelper.GetTextFromChild(textHolder,transform.parent.name);
        }
        else 
        {
            textMeshPro.text = TextHelper.GetTextFromChild(textHolder, key);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onUpdate) SetText();
    }
}
