using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadInfoCheck : MonoBehaviour
{
    [SerializeField]private AheadInfoDisplay displayLeft;
    [SerializeField]private AheadInfoDisplay displayRight;

    private bool stopAtStation = false;

    private Transform frontCheck;
    private Tram tram;
    private XPSystem xpSystem;

    private List<Transform> hitAhead=new List<Transform>();
    private AheadInfoDisplay displayFirst;//距离最近的display

    void Start()
    {
        frontCheck=Manager.instance.frontCheck;
        tram=Manager.instance.tram;
        xpSystem = Manager.instance.XPSystem;
    }

    void Update()
    {
        Collider2D _hit = Physics2D.OverlapPoint(frontCheck.position,1<<(int)SignType.warn);

        if (_hit != null)
        {
            Transform _hitAhead = _hit.transform.parent;
            if (!hitAhead.Contains(_hitAhead))
            {
                Enqueue(_hitAhead);
            }
        }

        CheckQueue();
    }

    private void Enqueue(Transform _hitAhead)
    {
        hitAhead.Add(_hitAhead);
        
        UpdateDisplay();

        switch ((SignType)_hitAhead.gameObject.layer)
        {
            case SignType.stop:
                Stop _stopInfo = _hitAhead.GetComponent<Stop>();
                if (!_stopInfo.passing)
                {
                    _stopInfo.station.SpawnPassenger(_stopInfo.station.population);
                    tram.GenerateUnboardList(_stopInfo.station);
                }
                break;

            case SignType.speedLimit:
                break;

            case SignType.signal:
                Signal signal = _hitAhead.GetComponent<Signal>();
                StartCoroutine(signal.StartChangingColor());
                break;

            default:
                break;
        }
    }

    private void Dequeue()
    {
        hitAhead.RemoveAt(0);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        hitAhead.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        if (hitAhead.Count <= 0)
        {
            displayLeft.SetDisplay(null);
            displayRight.SetDisplay(null);
            displayFirst=null;
        }
        else if (hitAhead.Count == 1)
        {
            displayLeft.SetDisplay(null);
            displayRight.SetDisplay(hitAhead[0]);
            displayFirst = displayRight;
        }
        else
        {
            displayLeft.SetDisplay(hitAhead[0]);
            displayRight.SetDisplay(hitAhead[1]);
            displayFirst = displayLeft;
        }
    }

    private void CheckQueue()
    {
        if (displayFirst == null) return;

        float _distance = displayFirst.distance;

        switch ((SignType)displayFirst.signType)
        {
            case SignType.stop:
                Stop _stopInfo = displayFirst.info.GetComponent<Stop>();

                if (_stopInfo.passing)
                {
                    if (_distance <= 0)
                    {
                        int _delay = (VirtualTime.CurrentTime() - _stopInfo.arrivalTime).ToInt();
                        xpSystem.StopPassed(_stopInfo.stopName, _delay, _stopInfo.maxAcceptDelay);
                        Dequeue();
                    }
                }//甩站
                else
                {
                    float _maxAcceptDeviation = _stopInfo.maxAcceptDeviation;
                    if (Mathf.Abs(tram.speed) < 0.01f && tram.doorOpen && MathF.Abs(_distance) < _maxAcceptDeviation)
                    {
                        if (stopAtStation == false)
                        {
                            int _delay = (VirtualTime.CurrentTime() - _stopInfo.arrivalTime).ToInt();
                            xpSystem.StopAtStation(_stopInfo.stopName, _distance, _maxAcceptDeviation, _delay, _stopInfo.maxAcceptDelay);
                        }
                        stopAtStation = true;

                        tram.UnboardPassenger(_stopInfo.station);//下客
                        _stopInfo.station.BoardPassenger();//上客
                    }//到站
                    if (_distance < -10f && stopAtStation)
                    {
                        int _delay = (VirtualTime.CurrentTime() - _stopInfo.departureTime).ToInt();
                        xpSystem.Depart(_stopInfo.stopName, _delay, _stopInfo.maxAcceptDelay * 2);
                        stopAtStation = false;

                        foreach (Passenger passenger in _stopInfo.station.passengerList)
                            Destroy(passenger.gameObject);

                        Dequeue();
                    }//出站
                    if (_distance < -50f && !stopAtStation)
                    {
                        xpSystem.StopMissed(_stopInfo.stopName);

                        foreach(Passenger passenger in _stopInfo.station.passengerList)
                            Destroy(passenger.gameObject);

                        Dequeue();
                    }//过站
                }//停靠站
                break;

            case SignType.speedLimit:
                SpeedLimit _speedLimit=displayFirst.info.GetComponent<SpeedLimit>();

                if (_distance <= 0)
                {
                    tram.speedLimit = _speedLimit.speedLimit;
                    Dequeue();
                }
                break;

            case SignType.signal:
                Signal signal=displayFirst.info.GetComponent<Signal>();
                
                if (_distance <= 0)
                {
                    switch (signal.signalColor)
                    {
                        case SignalColor.red:
                            xpSystem.RunThroughRedSignal();
                            break;
                        case SignalColor.yellow:
                            tram.signalSpeedLimit = GlobalVar.instance.signalSpeedLimit;
                            break;
                        case SignalColor.green:
                            tram.signalSpeedLimit = 1000;
                            break;
                    }
                    Dequeue();
                }
                break;
            default:break;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(frontCheck.position, new Vector3(frontCheck.position.x + checkDistance, frontCheck.position.y));
    }
}
