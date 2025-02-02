using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [SerializeField] private float walkingSpeedMin = 0.3f;
    [SerializeField] private float walkingSpeedMax = 0.5f;
    [SerializeField] private float animatorSpeed = 2f;
    private float walkingSpeed = 0.4f;
    public int walkingDirection = 0;

    private Transform tf;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private TramDoorController doorController;
    private Tram tram;
    private Vector3[] doors;
    private Coroutine coroutine=null;
    private bool busyBoarding = false;

    // Start is called before the first frame update
    void Start()
    {
        tram = Manager.instance.tram;
        doorController = tram.doorController;
        animator=GetComponentInChildren<Animator>();
        tf=GetComponent<Transform>();
        spriteRenderer= GetComponentInChildren<SpriteRenderer>();
        doors = new Vector3[]{
            doorController.door1LOffset,doorController.door1ROffset,doorController.door2Offset,
            doorController.door3LOffset,doorController.door3ROffset,doorController.door4Offset,
        };

        walkingSpeed=Random.Range(walkingSpeedMin, walkingSpeedMax);
        animator.speed= animatorSpeed* walkingSpeed*2/(walkingSpeedMin+walkingSpeedMax);
    }

    void Update()
    {
        animator.SetBool("Move", walkingDirection!=0);
        transform.rotation = Quaternion.Euler(0, walkingDirection < 0 ? 180 : 0, 0);//Flip

        if(Mathf.Abs(tram.speed) > 0.1f || tram.doorOpen == false)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);

                busyBoarding = false;
                walkingDirection = 0;
            }
        }
    }

    private Vector3 FindClosetDoorGlobal(Vector3[] _targetDoor)
    {
        Vector3 _ans=_targetDoor[0];
        float _minDistance = Mathf.Infinity;

        foreach (Vector3 _door in _targetDoor) {
            Vector3 _doorPosition = _door + new Vector3(doorController.transform.position.x, 0);
            float _distance = Mathf.Abs(tf.position.x - _doorPosition.x);
            if (_distance < _minDistance) { _minDistance = _distance; _ans = _doorPosition; }
        }
        return _ans;
    }
    private Vector3 FindClosetDoorLocal(Vector3[] _targetDoor)
    {
        Vector3 _ans=_targetDoor[0];
        float _minDistance = Mathf.Infinity;

        foreach (Vector3 _door in _targetDoor) {
            float _distance = Mathf.Abs(tf.localPosition.x - _door.x);
            if (_distance < _minDistance) { _minDistance = _distance; _ans = _door; }
        }
        return _ans;
    }

    public IEnumerator Board(Station _station)
    {
        if(busyBoarding)yield break;

        busyBoarding = true;
        Vector3 _door = FindClosetDoorGlobal(doors);

        yield return new WaitForSeconds(tram.doorController.openDuration);
        //等待门打开

        coroutine = StartCoroutine(WalkToGlobal(_door));
        yield return coroutine;
        coroutine = null;
        busyBoarding = false;

        _station.passengerList.Remove(this);
        transform.parent = tram.transform;
        spriteRenderer.sortingLayerName = "Tram";
        Manager.instance.XPSystem.PassengerBoard();

        int _sortingOrder = Random.Range(0,3);
        spriteRenderer.sortingOrder = -_sortingOrder;
        float _color = (float)_sortingOrder / 10;
        spriteRenderer.color = Color.white - new Color(_color, _color, _color,0);

        tram.passengerList.Add(this);

        float _cabinLenght = tram.length / 2;
        float _stand;
        do{
            _stand = Random.Range(0.1f * _cabinLenght, 0.9f * _cabinLenght);
            _stand = tf.localPosition.x > 0 ? _stand : -_stand;
        } while (Mathf.Abs(_stand - tf.localPosition.x) > _cabinLenght * 0.5f);

        StartCoroutine(WalkToLocal(new Vector3(_stand, 0)));

    }

    public IEnumerator Unbroad(Station _station)
    {
        if (busyBoarding) yield break;

        busyBoarding = true;
        Vector3 _door = FindClosetDoorLocal(doors);

        yield return new WaitForSeconds(tram.doorController.openDuration);
        //等待门打开
        coroutine = StartCoroutine(WalkToLocal(_door));
        yield return coroutine;
        coroutine = null;
        busyBoarding=false;

        tram.passengerUnboarding.Remove(this);
        transform.parent = _station.passengerParent;
        spriteRenderer.sortingLayerName = "Pedestrain";
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.color= Color.white;

        Manager.instance.XPSystem.PassengerBoard();

        Vector3 _exit = new Vector3( Random.value < 0.5 ? _station.stationLength : -_station.stationLength,0);

        yield return StartCoroutine(WalkToGlobal(_exit*0.8f + _station.transform.position));
        StartCoroutine(WalkToGlobal(_exit + _station.transform.position));
        StartCoroutine(Disappear());
    }

    private IEnumerator WalkToGlobal(Vector3 _target)
    {
        walkingDirection = tf.position.x - _target.x > 0 ? -1 : 1;

        while (Mathf.Abs(tf.position.x - _target.x) > 0.02f)
        {
            tf.position += new Vector3(Time.deltaTime * walkingDirection * walkingSpeed, 0);
            yield return null;
        }

        walkingDirection = 0;
    }
    private IEnumerator WalkToLocal(Vector3 _target)
    {
        walkingDirection = tf.localPosition.x - _target.x > 0 ? -1 : 1;

        while (Mathf.Abs(tf.localPosition.x - _target.x) > 0.02f)
        {
            tf.localPosition += new Vector3(Time.deltaTime * walkingDirection * walkingSpeed, 0);
            yield return null;
        }

        walkingDirection = 0;
    }

    private IEnumerator Disappear()
    {
        float _deltaTime = 0;
        while (_deltaTime < 1f)
        {
            _deltaTime += Time.deltaTime;
            spriteRenderer.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
