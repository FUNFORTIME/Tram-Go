using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XPSystem : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private TextMeshProUGUI XPdeltaText;
    [SerializeField] private GameObject message;//contain the texts

    [SerializeField] private float showDuration = 4f;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;

    [Header("Stop XP Adding")]
    [SerializeField] private int deviationMaxXP = 75;
    [SerializeField] private int deviationMinXP = 25;
    [SerializeField] private int delayMaxXP = 25;
    [SerializeField] private int delayMinXP = 0;

    private Coroutine descriptionCoroutine=null;
    private Coroutine updateXPCoroutine=null;

    public int XP=0;

    void Start()
    {
        XPdeltaText.text = "";
        //SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void Update()
    {
        
    }

    private int LerpXPCalculation(int _minXP,int _maxXP,float _delta,float _maxAcceptDelta)
    {
        float _proportion = Mathf.Clamp01(1 - Mathf.Abs(_delta) / _maxAcceptDelta);
        return (int)Mathf.Lerp(_minXP, _maxXP, Mathf.Pow(_proportion, 1.5f));
    }

    #region StopXP
    public void StopAtStation(Stop _stop, float _deviation,float _delay,float _maxAcceptDeviation,float _maxAcceptDelay=20f)
    {
        int _deviationXP = LerpXPCalculation(deviationMinXP,deviationMaxXP,_deviation,_maxAcceptDeviation);
        int _delayXP = LerpXPCalculation(delayMinXP,delayMaxXP,_delay,_maxAcceptDelay);

        //string _deviationDescription = $"{TXT("Deviation")}{(int)(_deviation * 100)}cm\t{TXT("BonusScore")} {_deviationXP}XP\n";
        //string _delayDescription = $"{TXT("Delay")} {(int)_delay}s\t{TXT("BonusScore")} {_delayXP}XP";

        XP += _delayXP + _deviationXP;

        StartCoroutine(
                UI.instance.stopResultDisplay.ShowStopResult(deviationMinXP, deviationMaxXP, delayMinXP, delayMaxXP, _stop, _deviation, _delay));
    }
    public void StopMissed(string _stopName)
    {
        ChangeXP(-100, $"{TXT("MissedStop")} {_stopName}");
    }

    public void StopPassed(string _stopName, float _delay,float _maxAcceptDelay = 20f)
    {
        int _delayXP = LerpXPCalculation(delayMinXP,delayMaxXP, _delay,_maxAcceptDelay);
 
        string _delayDescription = $"{TXT("Delay")}{_delay}s\t{TXT("BonusScore")}{_delayXP}XP";

        ChangeXP(_delayXP, $"{TXT("Passing")} {_stopName}\n{_delayDescription}");
    }
    public void Depart(string _stopName, float _delay, float _maxAcceptDelay = 20f)
    {
        int _delayXP = LerpXPCalculation(delayMinXP, delayMaxXP, _delay, _maxAcceptDelay);

        string _delayDescription = $"{TXT("Delay")}{_delay}s\t{TXT("BonusScore")}{_delayXP}XP";

        ChangeXP(_delayXP, $"{TXT("Depart")} { _stopName}\n{ _delayDescription}");
    }
    public void PassengerBoard()
    {
        ChangeXP(2, TXT("PassengerBoarding"));
    }
    #endregion

    public void EB()
    {
        ChangeXP(-50, TXT("EB"));
    }
    public void RunWithDoorOpen()
    {
        ChangeXP(-100, TXT("RunWithDoorOpen"));
    }

    public void RunThroughRedSignal()
    {
        ChangeXP(-200, TXT("RunThroughRedSignal"));
    }

    #region SpeedLimit
    public void ExceedSpeedLimit(int _speed,int _speedLimit)
    {
        ChangeXP(2*(_speedLimit-_speed)-10, $"{TXT("SLE")} {_speedLimit} km/h");
    }
    public void ConformingSpeedLimit(float _warnTime,float _conformTime)
    {
        ChangeXP(10 - (int)(_conformTime - _warnTime),
            $"{TXT("ConformingSpeedLimit")} {(int)(_conformTime - _warnTime)}s");
    }
    public void SpeedLimitNotConformed()
    {
        ChangeXP(-10, $"{TXT("SpeedLimitNotConformed")}");
    }
    #endregion

    public void AccelerationExceed()
    {
        ChangeXP(-20, TXT("AccelerationExceed"));
    }

    public void NoRingBellCloseDoor()
    {
        ChangeXP(-10, TXT("NoRing"));
    }

    public void ChangeXP(int _XP,string _description)
    {
        if(descriptionCoroutine!=null) StopCoroutine(descriptionCoroutine);
        if(updateXPCoroutine!=null) StopCoroutine(updateXPCoroutine);
        //This prevents the coroutines from running further if it is already active.

        if (_XP < 0)
            AudioManager.instance.PlaySFX(sfxType.punish, 1f, 0.5f);

        XP += _XP;
        Color _color = XP>=0? greenColor : redColor;

        descriptionCoroutine= StartCoroutine(UI.instance.descriptionController.ChangeDescription(_description,_color,showDuration));
        updateXPCoroutine= StartCoroutine(UpdateXPDisplay(_XP));
    }

    private IEnumerator UpdateXPDisplay(int _deltaXP)
    {
        XPText.text = XP.ToString() + "XP";
        XPdeltaText.color = _deltaXP >= 0 ? Color.green : Color.red;
        XPdeltaText.text = _deltaXP >= 0 ? "+" + _deltaXP.ToString() : "-" + _deltaXP.ToString();

        yield return new WaitForSeconds(showDuration);

        XPdeltaText.text = "";
    }

    //private void OnSceneChanged(Scene current, Scene next) => SaveXP();
    public void SaveXP()
    {
        if(XP>0)
            SaveManager.instance.gameData.xp += XP;

        if (SaveManager.instance.gameData.levelHighScore[LevelInfo.instance.level.GetString] < XP)
            SaveManager.instance.gameData.levelHighScore[LevelInfo.instance.level.GetString] = XP;

        SaveManager.instance.SaveGame();
    }

    private string TXT(string name)
    {
        string ret = TextHelper.GetTextFromChild(message, name);
        return ret;
    }
}
