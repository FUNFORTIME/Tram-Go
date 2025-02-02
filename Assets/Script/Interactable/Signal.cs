using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Signal : SignParent
{
    public SignalColor signalColor;
    [Tooltip("Set 0 to disable color change")] public int secondToChageColor=0;
    public SignalColor targetColor;

    [SerializeField] private SignalDisplay signalPanel;
    [SerializeField] private SignalDisplay signalWarnPanel;
    [SerializeField] private Transform warnSignal;

    private void Start()
    {
    }

    private void OnValidate()
    {
        foreach (SpriteRenderer renderer in warnSignal.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = showWarnSign;
        }

        signalPanel.ChangeColor(signalColor);
        if(showWarnSign) signalWarnPanel.ChangeColor(signalColor);

        warnSignal.position = this.transform.position - new Vector3(warnDistance, 0);
    }

    public IEnumerator StartChangingColor()
    {
        if(secondToChageColor==0) yield break;

        yield return new WaitForSeconds(secondToChageColor);

        signalColor = targetColor;
        signalPanel.ChangeColor(targetColor);
        if (warnDistance != 0) signalWarnPanel.ChangeColor(targetColor);

    }
}
