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

    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        medalImage.enabled = true;
        if(level.highScore<level.bronzeScore) medalImage.enabled = false;
        else if(level.highScore<level.silverScore) medalImage.sprite = bronzeMedal; 
        else if(level.highScore<level.goldScore) medalImage.sprite =silverMedal; 
        else medalImage.sprite = goldMedal;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SceneManager.LoadScene(level.scene.name));

        highScoreText.text = level.highScore.ToString() + "XP";
        levelText.text=level.levelName;
    }
}
