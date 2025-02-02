using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    void Update()
    {
        timeText.text=VirtualTime.CurrentTime().ToString();
    }
}
