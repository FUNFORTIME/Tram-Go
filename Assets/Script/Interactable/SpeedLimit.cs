using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedLimit : SignParent
{
    public int speedLimit;

    [SerializeField] private TextMeshPro signText;
    [SerializeField] private TextMeshPro warnText;
    [SerializeField] private Transform warn;

    // Start is called before the first frame update
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
        warn.localPosition = new Vector3(-warnDistance, 0);
    }
}
