using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReverseHandleDisplay : MonoBehaviour
{
    [SerializeField] private Transform handleBar;
    [SerializeField] private TextMeshProUGUI handleText;
    [SerializeField] private float handleWidth = 15f;
    [SerializeField] private AudioSource toggleAudioSource;
    private Vector3 positionOffset;

    private void Start()
    {
        positionOffset=handleBar.localPosition;
        SetReverse(false);
    }

    public void SetReverse(bool _reverse)
    {
        toggleAudioSource.Play();
        handleBar.localPosition = positionOffset + new Vector3(_reverse ? -handleWidth : handleWidth, 0);
        handleText.text = _reverse ? "R" : "D";
    }
}
