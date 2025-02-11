using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [SerializeField] private float walkingSpeedMin = 0.3f;
    [SerializeField] private float walkingSpeedMax = 0.5f;
    [SerializeField] private float animatorSpeed = 2f;
    [SerializeField][Tooltip("kg")] private float mass=50;
    [Tooltip("m/s")] private float walkingSpeed = 0.4f;
    public int walkingDirection = 0;
    [SerializeField] private AudioSource[] footstepSFX;


    private Transform tf;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CabinDoorController doorController;
    private Tram tram;
    private Cabin cabin=null;
    private Coroutine coroutine=null;
    private Coroutine footstepCoroutine = null;
    private bool busyBoarding = false;

    // Start is called before the first frame update
    void Start()
    {
        tram = Manager.instance.tram;
        animator=GetComponentInChildren<Animator>();
        tf=GetComponent<Transform>();
        spriteRenderer= GetComponentInChildren<SpriteRenderer>();

        mass *= Random.Range(0.8f, 1.2f);
        walkingSpeed=Random.Range(walkingSpeedMin, walkingSpeedMax);
        animator.speed= animatorSpeed* walkingSpeed*2/(walkingSpeedMin+walkingSpeedMax);
    }

    void Update()
    {
        animator.SetBool("Move", walkingDirection!=0);
        transform.rotation = Quaternion.Euler(0, walkingDirection < 0 ? 180 : 0, 0);//Flip

        if(Mathf.Abs(tram.speed) > 0.1f || tram.doorOpen == false)
        {
            if(footstepCoroutine!=null) 
                StopCoroutine(footstepCoroutine);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);

                busyBoarding = false;
                walkingDirection = 0;
            }
        }
    }

    private Transform FindClosetGlobal(Transform[] _targetList)
    {
        Transform _ans=_targetList[0];
        float _minDistance = Mathf.Infinity;

        for (int i = 0; i < _targetList.Length; i++) {
            Vector3 _globalPosition = _targetList[i].position;
            float _distance = Vector3.Distance(tf.position, _globalPosition);

            if (_distance < _minDistance)
            {
                _minDistance = _distance; 
                _ans = _targetList[i];
            }
        }
        return _ans;
    }
    private Vector3 FindClosetGlobal(Vector3[] _targetList,Vector3 _parent)
    {
        Vector3 _ans=_targetList[0];
        float _minDistance = Mathf.Infinity;

        for (int i = 0; i < _targetList.Length; i++) {
            Vector3 _globalPosition = _targetList[i]+_parent;
            float _distance = Vector3.Distance(tf.position, _globalPosition);

            if (_distance < _minDistance)
            {
                _minDistance = _distance; 
                _ans = _globalPosition;
            }
        }
        return _ans;
    }
    private Transform FindClosetLocal(Transform[] _targetList)
    {
        Transform _ans = _targetList[0];
        float _minDistance = Mathf.Infinity;

        for (int i = 0; i < _targetList.Length; i++)
        {
            Vector3 _localPosition = _targetList[i].localPosition;
            float _distance = Vector3.Distance(tf.localPosition, _localPosition);

            if (_distance < _minDistance)
            {
                _minDistance = _distance;
                _ans = _targetList[i];
            }
        }
        return _ans;
    }
    private Vector3 FindClosetLocal(Vector3[] _targetList)
    {
        Vector3 _ans = _targetList[0];
        float _minDistance = Mathf.Infinity;

        for (int i = 0; i < _targetList.Length; i++)
        {
            Vector3 _localPosition = _targetList[i];
            float _distance = Vector3.Distance(tf.localPosition, _localPosition);

            if (_distance < _minDistance)
            {
                _minDistance = _distance;
                _ans = _localPosition;
            }
        }
        return _ans;
    }

    public IEnumerator Board(Station _station)
    {
        if (busyBoarding) yield break;

        busyBoarding = true;
        cabin = FindClosetGlobal(tram.cabinList.Select(p => p.transform).ToArray()).GetComponent<Cabin>();
        Vector3 _door = FindClosetGlobal(
            cabin.doorController.doorInfo.Select(p => p.closePosition).ToArray(), cabin.doorController.transform.position);

        yield return tram.doorOpen;
        //等待门打开

        coroutine = StartCoroutine(WalkToGlobal(_door));
        yield return coroutine;
        coroutine = null;
        busyBoarding = false;

        _station.passengerList.Remove(this);
        transform.parent = cabin.transform;
        cabin.rb.mass += mass;
        spriteRenderer.sortingLayerName = "Tram";
        UI.instance.XPSystem.PassengerBoard();

        SetSortingOrder(Random.Range(0, 5));

        tram.passengerList.Add(this);

        float _stand;
        do
        {
            _stand = Random.Range(cabin.minStandPosition, cabin.maxStandPosition);
        } while (Mathf.Abs(_stand - tf.localPosition.x) > cabin.length / 2);

        StartCoroutine(WalkToLocal(new Vector3(_stand, 0)));

    }

    public void SetSortingOrder(int _sortingOrder)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sortingOrder = -_sortingOrder;
        float _color = (float)_sortingOrder / 20;
        spriteRenderer.color = Color.white - new Color(_color, _color, _color, 0);
    }

    public IEnumerator Unbroad(Station _station)
    {
        if (busyBoarding) yield break;

        busyBoarding = true;
        Vector3 _door = FindClosetLocal(
            cabin.doorController.doorInfo.Select(p=>p.closePosition).ToArray());

        yield return tram.doorOpen;
        //等待门打开
        coroutine = StartCoroutine(WalkToLocal(_door));
        yield return coroutine;
        coroutine = null;
        busyBoarding=false;

        tram.passengerUnboarding.Remove(this);
        transform.parent = _station.passengerParent;
        cabin.rb.mass -= mass;
        spriteRenderer.sortingLayerName = "Pedestrain";
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.color= Color.white;

        UI.instance.XPSystem.PassengerBoard();

        Vector3 _exit = new Vector3( Random.Range( -_station.stationLength/2 ,_station.stationLength/2),Random.Range(-1,0));

        yield return StartCoroutine(WalkToGlobal(_exit*0.8f + _station.transform.position));
        StartCoroutine(WalkToGlobal(_exit + _station.transform.position));
        StartCoroutine(Disappear());
    }

    private IEnumerator WalkToGlobal(Vector3 _target)
    {
        Vector3 _start = tf.position;
        walkingDirection = _start.x - _target.x > 0 ? -1 : 1;

        footstepCoroutine = StartCoroutine(PlayFootStepSFX(walkingSpeed));
        while (Vector3.Distance(tf.position,_target) > 0.02f)
        {
            tf.position += Time.deltaTime *walkingSpeed* Vector3.Normalize(_target - _start);
            yield return null;
        }
        StopCoroutine(footstepCoroutine);
        footstepCoroutine = null;

        walkingDirection = 0;
    }
    private IEnumerator WalkToLocal(Vector3 _target)
    {
        Vector3 _start = tf.localPosition;
        walkingDirection = _start.x - _target.x > 0 ? -1 : 1;

        footstepCoroutine = StartCoroutine(PlayFootStepSFX(walkingSpeed));
        while (Vector3.Distance(tf.localPosition,_target) > 0.02f)
        {
            tf.localPosition += Time.deltaTime * walkingSpeed * Vector3.Normalize(_target - _start);
            yield return null;
        }
        StopCoroutine(footstepCoroutine);
        footstepCoroutine = null;

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

    public IEnumerator PlayFootStepSFX(float _walkingSpeed, float _volume = 0.5f)
    {
        while (true)
        {
            int _sfxIndex = Random.Range(0, footstepSFX.Length - 1);
            footstepSFX[_sfxIndex].volume = 30f / mass;
            footstepSFX[_sfxIndex].pitch = 40f / mass;
            footstepSFX[_sfxIndex].Play();
            yield return new WaitForSeconds(1f / _walkingSpeed);
        }
    }


}
