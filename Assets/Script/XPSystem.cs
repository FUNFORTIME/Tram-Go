using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPSystem : MonoBehaviour ,ISaveManager
{
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private TextMeshProUGUI XPdeltaText;

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
    public void StopAtStation(string _stopName, float _deviation,float _maxAcceptDeviation,float _delay,float _maxAcceptDelay=20f)
    {
        int _deviationXP = LerpXPCalculation(deviationMinXP,deviationMaxXP,_deviation,_maxAcceptDeviation);
        int _delayXP = LerpXPCalculation(delayMinXP,delayMaxXP,_delay,_maxAcceptDelay);

        string _deviationDescription = "Deviation:" + (int)(_deviation * 100) + "cm\tBonus:" + _deviationXP + "XP\n";
        string _delayDescription = "Delay:" + (int)(_delay) + "s\tBonus:" + _delayXP + "XP";

        ChangeXP(_delayXP + _deviationXP, "Stopped at "+_stopName+"\n"+ _deviationDescription+_delayDescription);
    }//ͣ��վ
    public void StopMissed(string _stopName)
    {
        ChangeXP(-100,"Missed Stop:"+_stopName);
    }//���ͣ��վ
    public void StopPassed(string _stopName, float _delay,float _maxAcceptDelay = 20f)
    {
        int _delayXP = LerpXPCalculation(delayMinXP,delayMaxXP, _delay,_maxAcceptDelay);
 
        string _delayDescription = "Delay:" + (int)(_delay) + "s\tBonus:" + _delayXP + "XP";

        ChangeXP(_delayXP, "Passing " +_stopName+"\n"  +_delayDescription);
    }//ͨ��վ
    public void Depart(string _stopName, float _delay, float _maxAcceptDelay = 20f)
    {
        int _delayXP = LerpXPCalculation(delayMinXP, delayMaxXP, _delay, _maxAcceptDelay);

        string _delayDescription = "Delay:" + (int)(_delay) + "s\tBonus:" + _delayXP + "XP";

        ChangeXP(_delayXP, "Depart from "+ _stopName+"\n" + _delayDescription);
    }
    public void PassengerBoard()
    {
        ChangeXP(2, "Passenger Boarding");
    }
    #endregion

    public void EB()
    {
        ChangeXP(-50, "Emergency Brake Pulled");
    }//����EB
    public void RunWithDoorOpen()
    {
        ChangeXP(-100, "Run with doors open");
    }//δ�������

    public void RunThroughRedSignal()
    {
        ChangeXP(-200, "Run through red signal");
    }//�������

    public void ExceedSpeedLimit(int _speed,int _speedLimit)
    {
        ChangeXP(2*(_speedLimit-_speed)-10, "Speed Limit Exceed");
    }

    public void ChangeXP(int _XP,string _description)
    {
        if(descriptionCoroutine!=null) StopCoroutine(descriptionCoroutine);
        if(updateXPCoroutine!=null) StopCoroutine(updateXPCoroutine);
        //��������Э���Է����� ͬʱֻ����ʾһ��

        XP += _XP;
        Color _color = XP>=0? greenColor : redColor;

        descriptionCoroutine= StartCoroutine(Manager.instance.descriptionController.ChangeDescription(_description,_color,showDuration));
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

    public void LoadData(GameData _gameData)
    {
        this.XP = _gameData.xp;
        XPText.text = XP.ToString();
    }
    public void SaveData(ref GameData _gameData)
    {
        _gameData.xp = this.XP;
    }
}
