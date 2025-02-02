using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedLimitDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedLimitText;

    public void SetText(int _speedLimit)
    {
        speedLimitText.text = _speedLimit.ToString();
    }
}
