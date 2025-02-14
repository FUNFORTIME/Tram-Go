using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    public Level level;
    [SerializeField] private Image medalImage;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI unlockText;

    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        unlockText.text = level.unlock ? "" : level.xpToUnlock + "XP";

        button.onClick.RemoveAllListeners();
        if (level.unlock)
        {
            button.onClick.AddListener(() => SceneManager.LoadScene(level.routeName+"-"+level.levelName));
        }
        else
        {
            button.onClick.AddListener(() => UnlockConformDisplay.instance.ConformLevel(this));
        }

        medalImage.enabled = true;
        if(level.highScore<level.bronzeScore) medalImage.enabled = false;
        else if(level.highScore<level.silverScore) medalImage.sprite = bronzeMedal; 
        else if(level.highScore<level.goldScore) medalImage.sprite =silverMedal; 
        else medalImage.sprite = goldMedal;
       
        highScoreText.text = "HighScore: "+ level.highScore.ToString() + "XP";
        levelText.text=level.levelName;
    }

    public void Unlock()
    {
        SaveManager.instance.gameData.xp -= level.xpToUnlock;
        SaveManager.instance.gameData.levelUnlock[level.GetString] = true;
        SaveManager.instance.SaveGame();

        XPDisplay.instance.UpdateXPDisplay();
        level.unlock = true;
        UpdateDisplay();
    }


}
