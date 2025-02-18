using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StopResultDisplay : MonoBehaviour
{
    [SerializeField] private Scrollbar deviationBar;
    [SerializeField] private Scrollbar delayBar;

    [SerializeField] private TextMeshProUGUI stationNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI deviationText;
    [SerializeField] private TextMeshProUGUI delayText;
    [SerializeField] private TextMeshProUGUI maxDeviationTextL;
    [SerializeField] private TextMeshProUGUI maxDeviationTextR;
    [SerializeField] private TextMeshProUGUI maxDelayTextL;
    [SerializeField] private TextMeshProUGUI maxDelayTextR;

    [SerializeField] private GameObject stopMessage;

    public IEnumerator ShowStopResult(int minDeviationXP, int maxDeviationXP, int minDelayXP, int maxDelayXP, Stop stop, float deviation, float delay)
    {
        gameObject.SetActive(true);

        static int LerpXPCalculation(int _minXP, int _maxXP, float _delta, float _maxAcceptDelta)
        {
            float _proportion = Mathf.Clamp01(1 - Mathf.Abs(_delta) / _maxAcceptDelta);
            return (int)Mathf.Lerp(_minXP, _maxXP, Mathf.Pow(_proportion, 1.5f));
        }
        static string DistanceText(float _distance)
        {
            if (Mathf.Abs(_distance) >= 10)
                return ((int)_distance).ToString() + "m";
            else
                return ((int)(_distance * 100)).ToString() + "cm";
        }
        static string DelayText(float _delay)
        {
            return (int)_delay + "s";
        }

        float _startTime = Time.time;
        float _deviation = -stop.maxAcceptDeviation;
        float _delay = -stop.maxAcceptDelay;

        stationNameText.text = stop.GetLocalizedText();

        maxDeviationTextL.text = DistanceText(+stop.maxAcceptDeviation);
        maxDeviationTextR.text = DistanceText(-stop.maxAcceptDeviation);
        maxDelayTextL.text = DelayText(-stop.maxAcceptDelay);
        maxDelayTextR.text = DelayText(+stop.maxAcceptDelay);

        while (_deviation < deviation - 0.005f || _delay < delay - 0.5f)
        {
            deviationBar.value = -_deviation / stop.maxAcceptDeviation * 0.5f + 0.5f;
            delayBar.value = _delay / stop.maxAcceptDelay * 0.5f + 0.5f;

            deviationText.text = DistanceText(_deviation);
            delayText.text = DelayText(_delay);

            int _score = LerpXPCalculation(minDeviationXP, maxDeviationXP, _deviation, stop.maxAcceptDeviation) + LerpXPCalculation(minDelayXP, maxDelayXP, _delay, stop.maxAcceptDelay);
            scoreText.text = $"{scoreText.text} {_score} XP";

            _deviation += Time.deltaTime * (Time.time - _startTime + 0.2f) * (deviation - _deviation) * 0.5f;
            _delay += Time.deltaTime * (Time.time - _startTime + 0.2f) * (delay - _delay) * 0.5f;
            _deviation = Mathf.Clamp(_deviation, -stop.maxAcceptDeviation, +stop.maxAcceptDeviation);
            _delay = Mathf.Clamp(_delay, -stop.maxAcceptDelay, +stop.maxAcceptDelay);

            yield return null;
        }
    }

    private string TXT(string name)
    {
        string ret = TextHelper.GetTextFromChild(stopMessage, name);
        if (ret == null|| ret == "") ret=TextHelper.GetTextFromChild(stopMessage, "null");
        return ret;
    }

}
