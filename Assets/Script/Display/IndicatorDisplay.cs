using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorDisplay : MonoBehaviour
{
    [SerializeField] private MeterDisplay speedMeter;
    [SerializeField] private SpeedLimitDisplay speedLimitDisplay;

    private Tram tram;
    private GameMode gameMode = GameMode.normal;

    void Start()
    {
        tram = Manager.instance.tram;
        gameMode = SaveManager.instance.gameData.gameMode;

        switch (gameMode)
        {
            case GameMode.real:
                break;
            case GameMode.hard:
                speedLimitDisplay.gameObject.SetActive(false);
                break;
            case GameMode.normal:
                speedLimitDisplay.gameObject.SetActive(true);
                break;
        }
    }

    void Update()
    {
        speedMeter.value = tram.speed;
        int _speedLimit = Mathf.Min(tram.signalSpeedLimit, tram.speedLimit);
        speedLimitDisplay.SetText(_speedLimit>999?-1:_speedLimit);
    }
}
