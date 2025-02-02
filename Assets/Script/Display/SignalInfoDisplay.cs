using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignalInfoDisplay : MonoBehaviour
{
    [SerializeField] private Image redLight;
    [SerializeField] private Image yellowLight;
    [SerializeField] private Image greenLight;


    public void SetSignal(SignalColor color)
    {
        ChangeColor(color);
    }

    public void ChangeColor(SignalColor target)
    {
        redLight.color = Color.red - new Color(0, 0, 0, 0.9f);
        yellowLight.color = Color.yellow - new Color(0, 0, 0, 0.9f);
        greenLight.color = Color.green - new Color(0, 0, 0, 0.9f);

        switch (target)
        {
            case SignalColor.red: redLight.color = Color.red; break;
            case SignalColor.yellow: yellowLight.color = Color.yellow; break;
            case SignalColor.green: greenLight.color = Color.green; break;
        }
    }
}
