using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopInfoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textStationName;

    public void SetText(string _stationName)
    {
        textStationName.text = _stationName;
    }
}
