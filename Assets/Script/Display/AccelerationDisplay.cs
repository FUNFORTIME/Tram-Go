using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerationDisplay : MonoBehaviour
{
    [SerializeField] private Image fillArea;
    [SerializeField] private Slider slider;
    private Tram tram;

    void Start()
    {
        tram=Manager.instance.tram;
    }

    void Update()
    {
        slider.value = Mathf.Abs( tram.acceleration / 2f);
        fillArea.color = Color.Lerp(Color.green, Color.red,slider.value);
    }
}
