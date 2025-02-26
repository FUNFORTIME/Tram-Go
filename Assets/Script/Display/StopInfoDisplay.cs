using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textStationName;
    [SerializeField] private TextMeshProUGUI maxAcceptDeviationText;
    [SerializeField] private GameObject textHolder;


    public void SetText(Stop _stop)
    {
        string GetDistanceText(float _distance)
        {
            string _textDistance;
            if (Mathf.Abs(_distance) >= 10)
                _textDistance = ((int)_distance).ToString() + "m";
            else
                _textDistance = ((int)(_distance * 100)).ToString() + "cm";

            return _textDistance;
        }

        textStationName.text = _stop.GetLocalizedText();
        maxAcceptDeviationText.text = "(±" + GetDistanceText(_stop.maxAcceptDeviation) + ")";
        //textStationName.text = TextHelper.GetTextFromChild(textHolder,_stationName);
    }
}
