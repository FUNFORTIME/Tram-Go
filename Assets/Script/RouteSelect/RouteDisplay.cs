using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteDisplay : MonoBehaviour
{
    public Route route;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI routeText;
    [SerializeField]private TextMeshProUGUI unlockText;

    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite bronzeMedal;
    [SerializeField]private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;

    [SerializeField] private GameObject routeNameHolder;

    private void Start()
    {
        GameData gameData = SaveManager.instance.gameData;
        foreach (Level level in route.levelList)
        {
            if (!gameData.levelHighScore.ContainsKey(level.GetString))
                gameData.levelHighScore.Add(level.GetString, 0);

            if (!gameData.levelUnlock.ContainsKey(level.GetString))
                gameData.levelUnlock.Add(level.GetString, level.xpToUnlock==0);

            level.highScore = gameData.levelHighScore[level.GetString];
        }
        SaveManager.instance.gameData = gameData;
        SaveManager.instance.SaveGame();

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        routeText.text = TextHelper.GetTextFromChild(routeNameHolder, name);
        unlockText.text = route.unlock ? "" : route.xpToUnlock + "XP";

        button.onClick.RemoveAllListeners();
        if(route.unlock)
        {
            button.onClick.AddListener(()=>LevelManager.instance.Show(route));
        }
        else
        {
            button.onClick.AddListener(() => UnlockConformDisplay.instance.ConformRoute(this));
        }

        bool _bronzeAchieve = true;
        bool _silverAchieve = true;
        bool _goldAchieve = true;
        foreach(Level level in route.levelList)
        {
            if (level.highScore<level.bronzeScore)_bronzeAchieve=false;
            if (level.highScore<level.silverScore)_silverAchieve=false;
            if(level.highScore<level.goldScore)_goldAchieve=false;
        }

        medalImage.enabled = _bronzeAchieve || _silverAchieve || _goldAchieve;
        if (_bronzeAchieve) medalImage.sprite = bronzeMedal;
        if (_silverAchieve) medalImage.sprite = silverMedal;
        if (_goldAchieve) medalImage.sprite = goldMedal;
    }

    public void Unlock()
    {
        SaveManager.instance.gameData.xp -= route.xpToUnlock;
        SaveManager.instance.gameData.routeUnlock[route.routeName] = true;
        SaveManager.instance.SaveGame();

        XPDisplay.instance.UpdateXPDisplay();
        route.unlock = true;
        UpdateDisplay();
    }

    private void OnValidate()
    {
        
    }

}
