using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AheadInfoDisplay : MonoBehaviour
{
    public GameObject info=null;
    public float distance;
    public int signType;

    [Header("Panel")]
    [SerializeField] private GameObject stopInfoDisplay;
    [SerializeField] private GameObject speedLimitDisplay;
    [SerializeField]private GameObject signalInfoDisplay;

    [SerializeField] private TextMeshProUGUI textDistance;

    private Image background;
    private Transform tf;
    private Transform frontCheck;

    void Start()
    {
        frontCheck=Manager.instance.frontCheck;
        background= GetComponent<Image>();
    }

    public void SetDisplay(Transform _info)
    {
        if (_info==null)
        {
            info = null;
            background.enabled = false;
            signType = 0;
            tf=null;
        }
        else
        {
            info= _info.gameObject;
            background.enabled = true;
            signType = info.layer;
            tf = _info;
            Update();
        }
    }
    
    void Update()
    {
        textDistance.text = "";
        stopInfoDisplay.SetActive(false);
        speedLimitDisplay.SetActive(false);
        signalInfoDisplay.SetActive(false);

        if (info == null)
            return;

        distance = tf.position.x - frontCheck.position.x;
        SetDistanceText(distance);

        switch ((SignType)signType)
        {
            case SignType.stop:
                stopInfoDisplay.SetActive(true);
                Stop _stopinfo = info.GetComponent<Stop>();
                stopInfoDisplay.GetComponent<StopInfoDisplay>().SetText(_stopinfo.stopName);
                break;

            case SignType.speedLimit:
                speedLimitDisplay.SetActive(true);
                SpeedLimit _speedLimit = info.GetComponent<SpeedLimit>();
                speedLimitDisplay.GetComponent<SpeedLimitDisplay>().SetText(_speedLimit.speedLimit);
                break;

            case SignType.signal:
                signalInfoDisplay.SetActive(true);
                Signal _signal = info.GetComponent<Signal>();
                signalInfoDisplay.GetComponent<SignalInfoDisplay>().SetSignal(_signal.signalColor);
                break;

        }
    }

    private void SetDistanceText(float _distance)
    {
        string _textDistance;
        if (Mathf.Abs(_distance) >= 10)
            _textDistance = ((int)_distance).ToString() + "m";
        else
            _textDistance = ((int)(_distance * 100)).ToString() + "cm";

        textDistance.text = _textDistance;
    }

}
