using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TramDoorController : MonoBehaviour
{
    [SerializeField] private float doorWidth = 0.14f;
    public float openDuration = 1f;

    [Header("Door Access")]
    [SerializeField] private Transform door1L;
    [SerializeField] private Transform door1R;
    [SerializeField] private Transform door2;
    [SerializeField] private Transform door3L;
    [SerializeField] private Transform door3R;
    [SerializeField] private Transform door4;

    public Vector3 door1LOffset { get; private set; }
    public Vector3 door1ROffset {  get; private set; }
    public Vector3 door2Offset {  get; private set; }
    public Vector3 door3LOffset {  get; private set; }
    public Vector3 door3ROffset {  get; private set; }
    public Vector3 door4Offset {  get; private set; }

    private int doorOpening;
    private bool doorOpen;

    private void Start()
    {
        door1LOffset = door1L.localPosition;
        door1ROffset = door1R.localPosition;
        door2Offset = door2.localPosition;
        door3LOffset = door3L.localPosition;
        door3ROffset = door3R.localPosition;
        door4Offset = door4.localPosition;
    }

    public IEnumerator ChangeDoorState(bool _doorOpen)
    {
        if (doorOpening != 0) yield break;

        Manager.instance.tram.doorOpen = _doorOpen;

        float _startTime = Time.time;
        float _duration = 0;
        doorOpening = _doorOpen ? 1 : -1;

        while (_duration<openDuration)
        {
            _duration = Time.time - _startTime;
            float _proportion = _doorOpen? _duration/openDuration:1- _duration / openDuration;
            SetDoorPosition(_proportion);

            yield return null;
        }

        SetDoorPosition(_doorOpen ? 1:0);

        doorOpening = 0;
    }

    private void SetDoorPosition(float _proportion)
    {
        float _delta = Mathf.LerpUnclamped(0, doorWidth, _proportion);
        Vector3 _moved = new Vector3(_delta, 0, 0);

        door1L.localPosition = door1LOffset - _moved;
        door1R.localPosition = door1ROffset + _moved;
        door2.localPosition = door2Offset - _moved;
        door3L.localPosition = door3LOffset - _moved;
        door3R.localPosition = door3ROffset + _moved;
        door4.localPosition = door4Offset - _moved;
    }

    private void Update()
    {


    }

}
