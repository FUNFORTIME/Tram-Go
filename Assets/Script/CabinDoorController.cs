using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CabinDoorController : MonoBehaviour
{
    [Serializable]
    public struct DoorInfo
    {
        public Transform door;
        public Vector3 openPosition;
        public Vector3 closePosition;
    }

    public List<DoorInfo> doorInfo;

    [SerializeField] private bool doorOpen;
    public int doorOpening;

    private Cabin cabin;

    private void Start()
    {
        cabin= GetComponentInParent<Cabin>();
    }

    public IEnumerator ChangeDoorState(bool _doorOpen)
    {
        if (doorOpening != 0) yield break;

        float _startTime = Time.timeSinceLevelLoad;
        float _duration = 0;
        doorOpening = _doorOpen ? -1 : 1;

        cabinSFXType _sfxIndex = _doorOpen ?  cabinSFXType.doorClose: cabinSFXType.doorOpen;
        cabin.PlaySFX(_sfxIndex, 1f,0.8f);

        float openDuration = cabin.cabinSFX[(int)_sfxIndex].clip.length*0.8f;
        while (_duration<openDuration)
        {
            _duration = Time.timeSinceLevelLoad - _startTime;
            float _proportion = _doorOpen? 1 - _duration / openDuration : _duration / openDuration;
            SetDoorPosition(_proportion);

            yield return null;
        }
        doorOpening = 0;

        SetDoorPosition(_doorOpen ? 0 : 1);

        Manager.instance.tram.doorOpen = !_doorOpen;
    }

    private void SetDoorPosition(float _proportion)
    {
        foreach(DoorInfo _doorInfo in doorInfo)
        {
            _doorInfo.door.localPosition = 
                (_doorInfo.openPosition - _doorInfo.closePosition) * _proportion+_doorInfo.closePosition;
        }
    }

    private void Update()
    {
    }

    private void OnValidate()
    {
        foreach(DoorInfo _doorInfo in doorInfo)
        {
            _doorInfo.door.localPosition =  doorOpen?_doorInfo.openPosition: _doorInfo.closePosition;
        }
    }

}
