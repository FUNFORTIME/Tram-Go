using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using Random = UnityEngine.Random;

public class Cabin : MonoBehaviour
{
    [Header("Current Statistic")]
    [Tooltip("kW")] public float power = 0f;
    [Tooltip("kW")] public float powerTarget = 0f;

    [Header("Cabin Statistic")]
    [Tooltip("kg")] public float mass;
    public int capacity;
    public int maxCapacity;
    public float length;
    public float minStandPosition;
    public float maxStandPosition;
    public float defaultFriction = 0.01f;
    [Tooltip("km/h")] public float thresholdSpeed = 20f;
    [Tooltip("s")] public float powerDelay = .5f;
    [Tooltip("kW")] public float maxPower;
    [Tooltip("kW")] public float maxBrake;
    public AudioSource[] cabinSFX;

    [Header("External Access")]
    public CabinDoorController doorController;

    private Tram tram;
    private Collider2D collider2d;
    [HideInInspector] public Rigidbody2D rb;
    private Coroutine coroutine=null;

    private void Start()
    {
        tram = Manager.instance.tram;
        rb= GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        rb.mass= mass;
    }

    private void Update()
    {
        AudioControl();
    }

    private void AudioControl()
    {
        float _speedProportion = tram.speed / tram.maxSpeed;
        float _powerProportion = power / maxPower;
        float _brakeProportion = -power / maxBrake;

        PlaySFX(cabinSFXType.idle,1f,0.2f);

        PlaySFX(cabinSFXType.motor, _powerProportion *0.4f+0.4f, Mathf.Min(0.3f,  _powerProportion*0.4f));

        if (powerTarget - power > maxPower / 5f)
            PlaySFX(cabinSFXType.motorRise, _powerProportion, _powerProportion*0.2f+0.2f);

        if (tram.speed > 1f)
        {
            PlaySFX(cabinSFXType.reduceSpeed, MathF.Min( 0.5f+ _brakeProportion,1.5f), _brakeProportion*_speedProportion*1.5f);
            PlaySFX(cabinSFXType.brake, 1f, _brakeProportion*(0.8f-_speedProportion) - 0.3f);
        }
        else
        {
            StopSFX(cabinSFXType.reduceSpeed);
            StopSFX(cabinSFXType.brake);

        }

        if(powerTarget==0&&power<-maxBrake/10f)
            PlaySFX(cabinSFXType.brakeRelese,1f+ _brakeProportion, _brakeProportion+0.2f);
        
        if(Random.value<0.002f*Time.deltaTime)
            if(coroutine==null)
                coroutine=StartCoroutine(PlayCompSFX(10f));
    }


    private void FixedUpdate()
    {
        power += Time.deltaTime / powerDelay * (powerTarget - power);

        int _reverse = tram.reverse ? -1 : 1;
        float _force = power * 1000f / Mathf.Max(thresholdSpeed, rb.velocity.magnitude * 3.6f);
        
        if (_force <= 0)
        {
            rb.sharedMaterial.friction = defaultFriction - _force / rb.mass;
            collider2d.sharedMaterial = rb.sharedMaterial;
        }
        else
        {
            rb.sharedMaterial.friction = defaultFriction;
            collider2d.sharedMaterial = rb.sharedMaterial;

            rb.AddForce(_reverse * _force * transform.right);
        }
    }

    public void ChangePower(float _proportion)
    {
        if (_proportion > 0) powerTarget = maxPower* _proportion;
        else powerTarget = maxBrake * _proportion;
    }

    private void OnDrawGizmos()
    {
        Vector3 _start = new Vector3(minStandPosition, 0) + transform.position;
        Vector3 _end = new Vector3(maxStandPosition, 0) + transform.position;

        Gizmos.DrawLine( _start,_end);
    }

    private void OnValidate()
    {
        maxPower=Mathf.Abs(maxPower);
        maxBrake=Mathf.Abs(maxBrake);
    }

    private IEnumerator PlayCompSFX(float _duration,float _volume=0.2f)
    {
        PlaySFX(cabinSFXType.compBegin,1f,_volume);
        yield return new WaitForSeconds(cabinSFX[(int)cabinSFXType.compBegin].clip.length);

        PlaySFX(cabinSFXType.compLoop, 1f, _volume);
        yield return new WaitForSeconds(_duration);
        StopSFX(cabinSFXType.compLoop);

        PlaySFX(cabinSFXType.compEnd, 1f, _volume);
        yield return new WaitForSeconds(cabinSFX[(int)cabinSFXType.compEnd].clip.length);

        coroutine = null;
    }

    public bool PlaySFX(cabinSFXType _sfxIndex, float pitch = 1f, float volume = 1f)
        => AudioManager.instance.PlaySFX(cabinSFX[(int)_sfxIndex], pitch, volume);

    public void StopSFX(cabinSFXType _sfxIndex) => AudioManager.instance.StopSFX(cabinSFX[(int)_sfxIndex]);
}
