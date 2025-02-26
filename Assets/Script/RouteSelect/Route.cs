using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Route") ]
public class Route:ScriptableObject
{
    public bool unlock;
    public int xpToUnlock;
    public string routeName;
    public List<Level> levelList;
    [SerializeField] private GameObject routeNameHolder;
    public string GetLocalizedText()
    {
        return TextHelper.GetTextFromChild(routeNameHolder, routeName);
    }
}
