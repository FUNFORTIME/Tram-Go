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
    terminus=13,
    trigger=14,
}

public class LevelInfo : MonoBehaviour
{
    public static LevelInfo instance;

    public VirtualTime departureTime;
    public Level level;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
