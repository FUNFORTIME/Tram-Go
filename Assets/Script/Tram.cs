using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Tram : MonoBehaviour 
{
    [Header("Current Statistic")]
    [Tooltip("m/s2")] public float acceleration = 0f;
    [Tooltip("km/h")] public float speed = 0f;
    [Tooltip("km/h")] public int speedLimit = 1000;
    [Tooltip("km/h")] public int signalSpeedLimit = 1000;
    public int handle=0;
    public float handleProportion;
    public bool forward = true;
    public bool doorOpen = false;
    public float bellRingTime;//œÏ¡Â ±≥§
    public bool reverse = false;
    public List<Passenger> passengerList=new List<Passenger>();
    public List<Passenger> passengerUnboarding=new List<Passenger>();

    [Header("Tram Statistic")]
    public List<Cabin> cabinList;
    public int capacity = 208;
    public int maxCapacity = 324;
    [Range(0, 1)] public List<float> powerHandles;
    [Range(-2, 0)] public List<float> brakeHandles;
    [Tooltip("m")] public float length = 5f;
    [Tooltip("km/h")] public float maxSpeed = 120f;

    [Header("External Access")]
    [SerializeField] private HandleDisplay handleController;
    [SerializeField] private GameObject timeTable;

    private bool punishing = false;
    private Queue<float> speedRecord = new Queue<float>();

    void Start()
    {
        forward = true;
        doorOpen = false;
        reverse = false;
        StartCoroutine(Punishing());
        passengerList = new List<Passenger>();
        passengerUnboarding = new List<Passenger>();

        for (int i = 0; i < 50; i++)
            speedRecord.Enqueue(0);
    }

    private void FixedUpdate()
    {
        float _lastFrameSpeed = speed;
        speed = cabinList[0].rb.velocity.magnitude * 3.6f;

        acceleration = (speedRecord.Dequeue()-speed)/speedRecord.Count / 3.6f / Time.fixedDeltaTime;
        speedRecord.Enqueue(speed);

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
            handle = 0;
        }
    }

    void Update()
    {
        InputControl();
        AudioControl();

        if (punishing==false)
            XPCheck();
    }

    private void AudioControl()
    {
        float _speedProportion = speed / maxSpeed;

        AudioManager.instance.PlaySFX(sfxType.airLow,1f, 0.5f*Mathf.Sin(_speedProportion * 1.57f));
        AudioManager.instance.PlaySFX(sfxType.airHigh,0.5f+_speedProportion,_speedProportion);
    }

    private void XPCheck()
    {
        XPSystem xpSystem = UI.instance.XPSystem;
        if (doorOpen && Mathf.Abs(speed) > 0.1f)
        {
            xpSystem.RunWithDoorOpen();
            StartCoroutine(Punishing());
        }//RunWithDoorOpen
        if (handle <= -brakeHandles.Count&&Mathf.Abs(speed)>1f)
        {
            xpSystem.EB();
            StartCoroutine(Punishing());
        }//PullingEB
        if (speed > Mathf.Min(speedLimit,signalSpeedLimit))
        {
            xpSystem.ExceedSpeedLimit((int)speed, Mathf.Min(speedLimit, signalSpeedLimit));
            StartCoroutine(Punishing());
        }//SpeedLimitExceed
        if (Mathf.Abs(acceleration) > 2f)
        {
            xpSystem.AccelerationExceed();
            StartCoroutine(Punishing());
        }
    }

    private IEnumerator Punishing()
    {
        punishing = true;
        yield return new WaitForSeconds(10);
        punishing = false;
    }

    private void InputControl()
    {
        void HandleControl()
        {
            handle = Mathf.Clamp(handle, -brakeHandles.Count, powerHandles.Count);
            handleController.ChangeHandle(handle);

            if (handle == 0) handleProportion = 0;
            else if(handle>0) handleProportion=powerHandles[handle-1];
            else handleProportion = brakeHandles[-handle-1];

            foreach(Cabin _cabin in cabinList)
                _cabin.ChangePower(handleProportion);
        }

        if (!UI.instance.pause.pause)
        {
            if (Input.GetKeyDown(KeyCode.O))
                if (Mathf.Abs(speed) < 0.1f || doorOpen)
                {
                    bool _doorOpening = false;
                    foreach (Cabin cabin in cabinList)
                        _doorOpening = cabin.doorController.doorOpening!=0 || _doorOpening;

                    if (_doorOpening == false)
                    {
                        if(bellRingTime<2f&&doorOpen==true&&UI.instance.aheadInfoCheck.stopAtStation)
                            UI.instance.XPSystem.NoRingBellCloseDoor();

                        for (int i = 0; i < cabinList.Count; i++)
                        {
                            CabinDoorController doorController = cabinList[i].doorController;
                            StartCoroutine(doorController.ChangeDoorState(doorOpen));
                        }
                    }
                }
            //DoorControl

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                handle--;
                HandleControl();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                handle++;
                HandleControl();
            }
            //HandleControl

            if (Input.GetKeyDown(KeyCode.R) && Mathf.Abs(speed) < 0.1f)
            {
                reverse = !reverse;
                UI.instance.reverseHandleController.SetReverse(reverse);
            }
            //Reverse

            if (Input.GetKeyDown(KeyCode.T))
            {
                timeTable.gameObject.SetActive(!timeTable.gameObject.activeSelf);
            }
            //Show/Hide Timetable

            if (Input.GetKeyDown(KeyCode.C))
            {
                UI.instance.aheadInfoCheck.ConformSpeedLimit();
            }//Conforming SpeedLimit


            if (Input.GetKey(KeyCode.B))
                bellRingTime += Time.deltaTime;
            else
                bellRingTime = 0;
            //Ring DoorCloseBell


        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.instance.pause.ChangePause();
        }//Pause
    }

    public void GenerateUnboardList(Station _station,bool _terminus)
    {
        if (_terminus)
        {
            passengerUnboarding.AddRange(passengerList);
            return;
        }

        int _passengerNum = passengerList.Count;
        int _num = (int)((float)_passengerNum * (float)_station.populationUnboard/(float)_station.populationBoard);
        _num = (int)Mathf.Clamp(_num * Random.Range(0.5f, 1.5f), 0, _passengerNum);

        for (int i = 0; i < _num; i++)
            passengerUnboarding.Add(passengerList[i]);

        passengerList.RemoveRange(0, _num);
    }

    public void UnboardPassenger(Station _station)
    {
        foreach(Passenger passenger in passengerUnboarding)
            StartCoroutine(passenger.Unbroad(_station));
    }

    private void OnValidate()
    {
        capacity = 0;
        maxCapacity = 0;
        length = 0;
        foreach(Cabin cabin in cabinList)
        {
            capacity += cabin.capacity;
            maxCapacity += cabin.maxCapacity;
            length += cabin.length;
        }
    }
}
