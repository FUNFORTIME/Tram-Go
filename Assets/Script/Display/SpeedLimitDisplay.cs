using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedLimitDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedLimitText;
    [SerializeField] private TextMeshProUGUI conformText;

    [SerializeField] private GameObject Messages;

    public void SetText(int _speedLimit,bool _conformed)
    {
        speedLimitText.text = _speedLimit.ToString();
        conformText.text = _conformed ? 
            TextHelper.GetTextFromChild(Messages, "ConformedText") : 
            TextHelper.GetTextFromChild(Messages, "ConformingText");
    }
}
