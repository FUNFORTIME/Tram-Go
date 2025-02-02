using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignType
{
    warn=9,
    stop=10,
    speedLimit=11,
    signal=12,
}

public class GlobalVar : MonoBehaviour
{
    public static GlobalVar instance;

    public int signalSpeedLimit = 60;

    public VirtualTime departureTime;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
