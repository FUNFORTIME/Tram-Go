using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Stop : SignParent
{
    public string stopName;
    public VirtualTime arrivalTime;
    [HideInInspector] public VirtualTime departureTime;
    public int stoppingSecond=30;
    public float maxAcceptDeviation=5f;
    public float maxAcceptDelay = 20f;
    public bool passing = false;
    public bool terminus = false;

    public Signal depSignal;
    [SerializeField] private Signal arrSignal;
    [SerializeField] private Transform warnSign;
    public TextMeshPro stopText;

    public Station station {  get; private set; }

    private void Awake()
    {
        //pass
        station = GetComponentInParent<Station>();
        //SetStopName();
        //stopText.text = GetLocalizedText();
        //Debug.Log($"{stopName}:{stopText.text}");
    }



    private void Update()
    {
        bool bug = true;
        if (bug)
        {
            stopText.text = GetLocalizedText();
            bug = false;
        }
       
       //Debug.Log($"stopname: {stopName}\ntext:{stopText.text}");
    }

    private void Start()
    {
        //station = GetComponentInParent<Station>();
        
        stopText.text = GetLocalizedText();
        //localizedText = transform.parent.GetComponent<Station>().SetStationName();
        //stopText.text = transform.parent.GetComponent<Station>().SetStationName();
    }

    private void OnValidate()
    {
        warnSign.localPosition = new Vector3(-warnDistance, warnSign.localPosition.y);

        //transform.parent.name = stopName;
        
        if (arrSignal != null && depSignal != null)
        {
            if (passing)
            {
                depSignal.signalColor = SignalColor.green;
                depSignal.secondToChageColor = 0;

                arrSignal.signalColor = SignalColor.green;
                arrSignal.secondToChageColor = 0;
            }
            else
            {
                depSignal.signalColor = SignalColor.red;
                depSignal.targetColor = SignalColor.green;
                depSignal.secondToChageColor = 20;

                arrSignal.signalColor = SignalColor.yellow;
                arrSignal.secondToChageColor = 0;
            }
        }

        departureTime = arrivalTime + new VirtualTime(0, 0, stoppingSecond);
    }

    public string GetLocalizedText()
    {
        SetStopName();
        //Debug.Log($"stopname: {stopName}\ntext:{transform.parent.GetComponent<Station>().GetStationName()}");
        string ret= transform.parent.GetComponent<Station>().GetStationName();
        //Debug.LogWarning(ret);
        stopText.text = ret;
        return ret;
    }

    private void SetStopName()
    {
        if (transform.parent != null)
        {
            stopName = transform.parent.name;
        }
        else
        {
            stopName = "DefaultName";
        }
    }
}
