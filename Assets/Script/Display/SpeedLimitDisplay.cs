using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedLimitDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedLimitText;
    [SerializeField] private TextMeshProUGUI conformText;
    [SerializeField]private bool showConformText=true;

    [SerializeField] private GameObject Messages;

    public void SetText(int _speedLimit,bool _conformed=false)
    {
        speedLimitText.text = _speedLimit>0?_speedLimit.ToString():"";

        if (showConformText)
            conformText.text = _conformed ?
                TextHelper.GetTextFromChild(Messages, "ConformedText") :
                TextHelper.GetTextFromChild(Messages, "ConformingText");
        else
            conformText.text = "";
    }
}
