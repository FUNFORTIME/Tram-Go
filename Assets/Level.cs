using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct LevelInfo
{
    public string name;
    public SceneAsset scene;
    public int highScore;
}

public class Level : MonoBehaviour
{
    public LevelInfo levelInfo;
    public RouteDisplay route;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
/*        levelInfo =
        SaveManager.instance.gameData.levelInfos;
*/    }

    private void OnValidate()
    {
        name=levelInfo.name;
        levelText.text = levelInfo.name;
    }
}
