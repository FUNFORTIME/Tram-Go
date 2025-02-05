using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private Transform warnSign;
    [SerializeField] private TextMeshPro stopText;

    public Station station {  get; private set; }

    private void Awake()
    {
        //pass
        arrivalTime= new VirtualTime();
        departureTime= new VirtualTime();
    }

    private void Update()
    {

    }

    private void Start()
    {
        station = GetComponentInParent<Station>();
    }

    private void OnValidate()
    {
        warnSign.localPosition = new Vector3(-warnDistance, 0);

        transform.parent.name = stopName;
        stopText.text = stopName;
        

        departureTime = arrivalTime + new VirtualTime(0, 0, stoppingSecond);
    }
}
