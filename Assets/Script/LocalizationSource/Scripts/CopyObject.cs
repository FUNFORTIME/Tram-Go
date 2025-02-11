using LanguageLocalization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CopyObject : MonoBehaviour
{
    [SerializeField] private GameObject sourceObject;
    [SerializeField] private string nameFilePath;
    [SerializeField] private Transform parent;

    [SerializeField] private bool copyObjects;
    [SerializeField] private bool SetKey;
    // Start is called before the first frame update
    void Start()
    {
        if(copyObjects)CopyObjects();
        if(SetKey)SetKeyIDForAllChildren(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetKeyIDForAllChildren(GameObject parent)
    {
        // Get Localization_KEY Script
        Localization_KEY localizationKey = parent.GetComponent<Localization_KEY>();

        if (localizationKey != null)
        {
            // set keyID 
            localizationKey.keyID = parent.name;
        }

        
        foreach (Transform child in parent.transform)
        {
            SetKeyIDForAllChildren(child.gameObject);
        }
    }

    public void CopyObjects()
    {
        if (sourceObject == null)
        {
            Debug.LogError("sourceObject null!");
            return;
        }

        if (string.IsNullOrEmpty(nameFilePath))
        {
            Debug.LogError("nameFilePath null!");
            return;
        }

        string fullPath = Path.Combine(Application.dataPath, nameFilePath);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"No File!Path:{fullPath}");
            return;
        }

        List<string> nameList = new List<string>();
        using (StreamReader reader = new StreamReader(fullPath))
        {
            while (!reader.EndOfStream)
            {
                string name = reader.ReadLine().Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    nameList.Add(name);
                }
            }
        }

        foreach (string name in nameList)
        {
            GameObject copiedObject = Instantiate(sourceObject, parent);
            copiedObject.name = name;

            //// Save As Prefab
            //string prefabPath = Path.Combine("Assets/Prefabs", $"{name}.prefab");
            //ObjectSaver.SaveAsPrefab(copiedObject, prefabPath);

            //// Mark As Dirty
            //ObjectSaver.MarkAsDirty(copiedObject);
        }
    }
}

