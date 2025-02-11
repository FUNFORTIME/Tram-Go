using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeterDisplay : MonoBehaviour
{
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxAngle;

    public float value;
    [SerializeField] private Transform pointer;
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {

    }


    void Update()
    {
        if (pointer != null) 
            pointer.rotation = Quaternion.Euler(0,0,Mathf.Lerp(minAngle,maxAngle,(value-minValue)/(maxValue-minValue)));

        if (text != null)
            text.text=Mathf.RoundToInt(value).ToString();
    }
}
