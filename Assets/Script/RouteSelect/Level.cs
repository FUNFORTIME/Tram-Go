using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Level")]
public class Level :ScriptableObject
{
    public string routeName;
    public string levelName;
    public int xpToUnlock = 0 ;
    public bool unlock=false;
    //[TextArea] public string levelDescription;

    public int signalSpeedLimit = 60;

    public int highScore;
    public int maxScore;
    public int bronzeScore;
    public int silverScore;
    public int goldScore;

    public string GetString => routeName +"-"+ levelName;

    private void OnValidate()
    {
        name=routeName+"-"+levelName;
    }
}
