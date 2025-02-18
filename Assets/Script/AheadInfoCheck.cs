using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AheadInfoCheck : MonoBehaviour
{
    [SerializeField]private AheadInfoDisplay displayLeft;
    [SerializeField]private AheadInfoDisplay displayRight;

    public bool stopAtStation = false;

    private Transform frontCheck;
    private Tram tram;
    private XPSystem xpSystem;

    private List<Transform> hitAhead=new List<Transform>();
    private AheadInfoDisplay displayFirst;//display

    void Start()
    {
        frontCheck=Manager.instance.frontCheck;
        tram=Manager.instance.tram;
        xpSystem = UI.instance.XPSystem;
    }

    void Update()
    {
        Collider2D[] _hits = Physics2D.OverlapPointAll(frontCheck.position,1<<(int)SignType.warn);

        foreach (Collider2D _hit in _hits)
        {
            if (_hit != null)
            {
                Transform _hitAhead = _hit.transform.parent;
                if (!hitAhead.Contains(_hitAhead))
                {
                    Enqueue(_hitAhead);
                }
            }

        }        
        
        CheckQueue(displayLeft);
        CheckQueue(displayRight);
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
                    _stopInfo.station.StartAudioPlay();

                    if(!_stopInfo.terminus)
                        _stopInfo.station.SpawnPassenger((int)(_stopInfo.station.populationBoard* UnityEngine.Random.Range(0.5f, 1.5f)));
                    tram.GenerateUnboardList(_stopInfo.station,_stopInfo.terminus);
                }
                break;

            case SignType.speedLimit:
                SpeedLimit _speedLimit = _hitAhead.GetComponent<SpeedLimit>();
                _speedLimit.warnTime = Time.timeSinceLevelLoad;
                break;

            case SignType.signal:
                Signal signal = _hitAhead.GetComponent<Signal>();
                StartCoroutine(signal.StartChangingColor());
                break;

            case SignType.terminus:
                break;

            case SignType.trigger:
                break;

            default:
                break;
        }
    }

    private void Dequeue(Transform _target)
    {
        hitAhead.Remove(_target);
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

    private void CheckQueue(AheadInfoDisplay display)
    {
        if (display == null) return;

        float _distance = display.distance;

        switch ((SignType)display.signType)
        {
            case SignType.stop:
                Stop _stopInfo = display.info.GetComponent<Stop>();

                if (_stopInfo.passing)
                {
                    if (_distance <= 0)
                    {
                        int _delay = (VirtualTime.CurrentTime() - _stopInfo.arrivalTime).ToInt();
                        xpSystem.StopPassed(_stopInfo.GetLocalizedText(), _delay, _stopInfo.maxAcceptDelay);
                        Dequeue(display.tf);
                    }
                }
                else
                {
                    float _maxAcceptDeviation = _stopInfo.maxAcceptDeviation;
                    float _depSignalDistance =  _stopInfo.depSignal==null?-10f: _stopInfo.transform.position.x - _stopInfo.depSignal.transform.position.x;
                    if (Mathf.Abs(tram.speed) < 0.01f && tram.doorOpen && MathF.Abs(_distance) < _maxAcceptDeviation)
                    {
                        if (stopAtStation == false)
                        {
                            int _delay = (VirtualTime.CurrentTime() - _stopInfo.arrivalTime).ToInt();
                            xpSystem.StopAtStation(_stopInfo, _distance, _delay,_stopInfo.maxAcceptDeviation, _stopInfo.maxAcceptDelay);
                        }
                        stopAtStation = true;

                        tram.UnboardPassenger(_stopInfo.station);
                        _stopInfo.station.BoardPassenger();
                    }
                    if (_distance < _depSignalDistance && stopAtStation)
                    {
                        int _delay = (VirtualTime.CurrentTime() - _stopInfo.departureTime).ToInt();
                        xpSystem.Depart(_stopInfo.GetLocalizedText(), _delay, _stopInfo.maxAcceptDelay * 2);
                        stopAtStation = false;

                        foreach (Passenger passenger in _stopInfo.station.passengerList)
                            Destroy(passenger.gameObject);

                        _stopInfo.station.StopAudioPlay();
                        Dequeue(display.tf);
                    }
                    if (_distance < -50f && !stopAtStation)
                    {
                        xpSystem.StopMissed(_stopInfo.GetLocalizedText());

                        foreach(Passenger passenger in _stopInfo.station.passengerList)
                            Destroy(passenger.gameObject);

                        _stopInfo.station.StopAudioPlay();
                        Dequeue(display.tf);
                    }
                }
                break;

            case SignType.speedLimit:
                SpeedLimit _speedLimit=display.info.GetComponent<SpeedLimit>();

                if (_distance <= 0)
                {
                    tram.speedLimit = _speedLimit.speedLimit;
                    if (_speedLimit.conformed == false)
                        xpSystem.SpeedLimitNotConformed();

                    AudioManager.instance.PlaySFX(sfxType.speedLimitUpdate, 1f, 0.5f);
                    Dequeue(display.tf);
                }
                break;

            case SignType.signal:
                Signal signal=display.info.GetComponent<Signal>();
                
                if (_distance <= 0)
                {
                    switch (signal.signalColor)
                    {
                        case SignalColor.red:
                            xpSystem.RunThroughRedSignal();
                            break;
                        case SignalColor.yellow:
                            //tram.signalSpeedLimit = LevelInfo.instance.level.signalSpeedLimit;
                            break;
                        case SignalColor.green:
                            tram.signalSpeedLimit = 1000;
                            break;
                    }
                    Dequeue(display.tf);
                }
                break;

            case SignType.terminus:
                if(MathF.Abs(tram.speed) < 0.01f||_distance<0)
                {
                    UI.instance.resultDisplay.gameObject.SetActive(true);
                    UI.instance.resultDisplay.ShowResult();
                    Dequeue(display.tf);
                }
                break;

            case SignType.trigger:
                if (_distance < 0)
                {
                    Trigger trigger = display.info.GetComponent<Trigger>();
                    trigger.CallTrigger();
                    Dequeue(display.tf);
                }
                break;


            default:break;
        }
    }

    public void ConformSpeedLimit()
    {
        if(displayLeft.ConformSpeedLimit()==false)
            displayRight.ConformSpeedLimit();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(frontCheck.position, new Vector3(frontCheck.position.x + checkDistance, frontCheck.position.y));
    }
}
