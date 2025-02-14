using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedLimit : SignParent
{
    public int speedLimit;
    public bool conformed = false;
    public float warnTime;

    [SerializeField] private TextMeshPro signText;
    [SerializeField] private TextMeshPro warnText;
    [SerializeField] private Transform warn;

    void Start()
    {
    }

    private void OnValidate()
    {
        foreach (SpriteRenderer renderer in warn.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = showWarnSign;
        }

        signText.text=speedLimit.ToString();
        warnText.text=showWarnSign? speedLimit.ToString():"";
        warn.localPosition = new Vector3(-warnDistance, warn.localPosition.y);
    }
}
