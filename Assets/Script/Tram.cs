using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Tram : MonoBehaviour 
{
    [Header("Current Statistic")]
    [Tooltip("km/h")] public float speed = 0f;
    [Tooltip("km/h")] public int speedLimit = 1000;
    [Tooltip("km/h")] public int signalSpeedLimit = 1000;
    [Tooltip("m/s2")] public float acceleration = 0f;
    [Tooltip("m/s2")] public float accelerationTarget = 0f;
    public bool forward = true;
    public bool doorOpen = false;
    public bool reverse = false;
    public List<Passenger> passengerList=new List<Passenger>();
    public List<Passenger> passengerUnboarding=new List<Passenger>();

    [Header("Tram Statistic")]
    public int capacity = 208;
    public int maxCapacity = 324;
    public float maxSpeed = 10f;
    public float accelerateDelay = .5f;
    public float length = 5f;
    public List<float> accelerateAbility;

    [Header("External Access")]
    [SerializeField] private MeterDisplay speedMeter;
    public TramDoorController doorController;
    [SerializeField] private HandleDisplay handleController;
    [SerializeField] private GameObject timeTable;

    private Rigidbody2D rb;
    private bool punishing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputControl();

        if (punishing==false)
            XPCheck();

        rb.velocity = new Vector2(reverse?-speed:speed, 0)/3.6f;
        speed += Time.deltaTime * acceleration*3.6f;
        if(speed> maxSpeed)
        {
            speed = maxSpeed;
            accelerationTarget = 0;
        }

        acceleration += Time.deltaTime/accelerateDelay * (accelerationTarget-acceleration);
        if (speed<.0001f&&acceleration < 0&&forward)
        {
            speed = 0;
            acceleration = 0;
            accelerationTarget= 0;
        }

        speedMeter.value = speed;
    }

    private void XPCheck()
    {
        XPSystem xpSystem = Manager.instance.XPSystem;
        if (doorOpen && Mathf.Abs(speed) > 0.1f)
        {
            xpSystem.RunWithDoorOpen();
            StartCoroutine(Punishing());
        }//RunWithDoorOpen
        if (handleController.handle <= -handleController.brakeNum&&Mathf.Abs(speed)>1f)
        {
            xpSystem.EB();
            StartCoroutine(Punishing());
        }//PullingEB
        if (speed > Mathf.Min(speedLimit,signalSpeedLimit))
        {
            xpSystem.ExceedSpeedLimit((int)speed, Mathf.Min(speedLimit, signalSpeedLimit));
            StartCoroutine(Punishing());
        }//SpeedLimitExceed
    }

    private IEnumerator Punishing()
    {
        punishing = true;
        yield return new WaitForSeconds(10);
        punishing = false;
    }

    private void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.O))
            if (Mathf.Abs(speed) < 0.1f || doorOpen)
            {
                StartCoroutine(doorController.ChangeDoorState(!doorOpen));
            }
        //DoorControl

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            handleController.ChangeHandle(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            handleController.ChangeHandle(1);
        //HandleControl

        if (Input.GetKeyDown(KeyCode.R) && Mathf.Abs(speed) < 0.1f)
        {
            reverse = !reverse;
            Manager.instance.reverseHandleController.SetReverse(reverse);
        }
        //Reverse

        if (Input.GetKeyDown(KeyCode.T))
        {
            timeTable.gameObject.SetActive(!timeTable.gameObject.activeSelf);
        }
        //Show/Hide Timetable
    }

    public void GenerateUnboardList(Station _station)
    {
        int _passengerNum = passengerList.Count;
        int _num = (int)((float)_passengerNum * 100f / (float)_station.population);
        _num = (int)Mathf.Clamp(_num * Random.Range(0f, 2f), 0, _passengerNum);

        for (int i = 0; i < _num; i++)
            passengerUnboarding.Add(passengerList[i]);

        passengerList.RemoveRange(0, _num);
    }

    public void UnboardPassenger(Station _station)
    {
        foreach(Passenger passenger in passengerUnboarding)
            StartCoroutine(passenger.Unbroad(_station));
    }
}
