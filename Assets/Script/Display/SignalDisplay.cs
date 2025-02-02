using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SignalColor
{
    red,
    yellow,
    green,
}

public class SignalDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer redLight;
    [SerializeField] private SpriteRenderer yellowLight;
    [SerializeField] private SpriteRenderer greenLight;

    public void ChangeColor(SignalColor target)
    {
        redLight.color = Color.red - new Color(0, 0, 0,0.9f);
        yellowLight.color = Color.yellow - new Color(0, 0,0, 0.9f);
        greenLight.color = Color.green - new Color(0, 0,0, 0.9f);

        switch(target)
        {
            case SignalColor.red : redLight.color = Color.red; break;
            case SignalColor.yellow : yellowLight.color = Color.yellow; break;
            case SignalColor.green : greenLight.color = Color.green; break;
        }
    }
}
