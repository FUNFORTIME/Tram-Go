using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tram : MonoBehaviour
{
    [Header("Current Statistic")]
    public float speed = 0f;
    public float acceleration = 0f;
    public float accelerationTarget = 0f;
    public bool forward = true;

    [Header("Tram Statistic")]
    public float maxSpeed = 20f;
    public float accelerateDelay = .5f;
    public float length = 25f;
    public List<float> accelerateAbility;

    [Header("Meter Access")]
    public MeterController speedMeter;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed, 0);
        speed += Time.deltaTime * acceleration;
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

        speedMeter.value = speed * 8;
    }
}
