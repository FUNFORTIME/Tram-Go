using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentXPText;
    [SerializeField] private Transform bronzeMedal;
    [SerializeField] private Transform silverMedal;
    [SerializeField] private Transform goldMedal;
    [SerializeField] private LevelInfo levelInfo;
    private int xp;
    private float width;
    private Level level;

    private void Start()
    {
    }

    public void ShowResult()
    {
        gameObject.SetActive(true);
        xp = UI.instance.XPSystem.XP;
        level = levelInfo.level;
        width=200;

        UI.instance.XPSystem.SaveXP();

        highScoreText.text = "High Score: "+level.highScore+"XP";

        bronzeMedal.localPosition = new Vector2((float)level.bronzeScore / (float)level.maxScore * width-width/2, 0);
        silverMedal.localPosition = new Vector2((float)level.silverScore / (float)level.maxScore * width - width / 2, 0);
        goldMedal.localPosition = new Vector2((float)level.goldScore / (float)level.maxScore * width - width / 2, 0);

        StartCoroutine(ShowXP());
    }

    private IEnumerator ShowXP()
    {
        float _xp = 0;
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlaySFX(sfxType.handClap);

        bool goldSFXPlayed = false;
        bool silverSFXPlayed = false;
        bool bronzeSFXPlayed = false;

        float _startTime = Time.time;
        while(_xp<xp-0.5f)
        {
            _xp += Time.deltaTime*(Time.time-_startTime+0.2f) * (xp - _xp)*0.5f;
            currentXPText.text = "This run:"+(int)_xp+"XP";
            slider.value = _xp / level.maxScore;

            if (_xp > level.goldScore && !goldSFXPlayed)
            {
                goldSFXPlayed = true;
                audioManager.PlaySFX(sfxType.getGoldMedal);
            }
            else if (_xp > level.silverScore && !silverSFXPlayed)
            {
                silverSFXPlayed = true;
                audioManager.PlaySFX(sfxType.getSilverMedal);
            }
            else if (_xp > level.bronzeScore && !bronzeSFXPlayed)
            {
                bronzeSFXPlayed = true;
                audioManager.PlaySFX(sfxType.getBronzeMedal);
            }

            yield return null;
        }

        _xp = xp;
        currentXPText.text = "This run:" + (int)_xp + "XP";
        slider.value = _xp / level.maxScore;

    }
}
