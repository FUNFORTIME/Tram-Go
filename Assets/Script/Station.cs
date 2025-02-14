using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Station : MonoBehaviour
{
    public float stationLength = 5f;
    public int populationBoard = 50;
    public int populationUnboard = 50;

    public List<Passenger> passengerList = new List<Passenger>();
    public UnityEngine.Transform passengerParent;
    [SerializeField] private GameObject passengerMale;
    [SerializeField] private GameObject passengerFemale;
    [SerializeField] private TextMeshPro stationName;
    private TextManager textManager;
    [SerializeField] private AudioSource[] stationSFX;
    private Tram tram;
    private stationSFXType sfxIndex;
    private Coroutine coroutine=null;

    public void SpawnPassenger(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            GameObject _prefab = Random.value < 0.5 ? passengerMale : passengerFemale;
            GameObject _passenger = Instantiate(_prefab, passengerParent);
            passengerList.Add(_passenger.GetComponent<Passenger>());

            int _sortingOrder = Random.Range(0,5);
            _passenger.transform.localPosition = new Vector3(Random.Range(-stationLength / 2, stationLength / 2), 0.1f*_sortingOrder-0.5f);
            _passenger.GetComponent<Passenger>().SetSortingOrder(_sortingOrder);
        }
    }

    public void BoardPassenger()
    {
        int _boardNum = Mathf.Min(passengerList.Count,tram.maxCapacity-tram.passengerList.Count);

        for (int i = 0; i < _boardNum; i++)
            StartCoroutine(passengerList[i].Board(this));
    }

    public void DestroyPassenger()
    {
        foreach (Passenger passenger in passengerList)
        {
            Destroy(passenger);
            passengerList.Remove(passenger);
        }
    }

    void Start()
    {
        tram = Manager.instance.tram;
        SetStationName();
    }

    public void StartAudioPlay()
    {
        sfxIndex = stationSFXType.lowPopulation;
        if (populationBoard > 75)
            sfxIndex = stationSFXType.mediumPopulation;
        else if (populationBoard > 150)
            sfxIndex = stationSFXType.highPopulation;

        PlaySFX(sfxIndex,1f,0.5f + populationBoard / 300);

        coroutine= StartCoroutine(AudioUpdate());
    }

    public void StopAudioPlay()
    {
        StopSFX(stationSFXType.doorCloseBell);
        StopCoroutine(coroutine);
    }

    private IEnumerator AudioUpdate()
    {
        while (true)
        {
            float _volume= 10f/Vector2.Distance(tram.cabinList[0].transform.position, transform.position);
            stationSFX[(int)sfxIndex].volume = Mathf.Clamp01(_volume);

            if (tram.bellRingTime > 0)
                PlaySFX(stationSFXType.doorCloseBell);
            else
                StopSFX(stationSFXType.doorCloseBell);

            yield return null;
        }
    }

    void Update()
    {
        SetStationName();
    }
    private void OnValidate()
    {
        //stationName.text = name;
    }

    public bool PlaySFX(stationSFXType _sfxIndex, float pitch = 1f, float volume = 1f)
    => AudioManager.instance.PlaySFX(stationSFX[(int)_sfxIndex], pitch, volume);

    public void StopSFX(stationSFXType _sfxIndex) => AudioManager.instance.StopSFX(stationSFX[(int)_sfxIndex]);

    public void SetStationName()
    {
        TextManager textManager = transform.parent.GetComponent<TextManager>();
        if (textManager != null && textManager.textHolder != null)
        {
            GameObject textHolder = textManager.textHolder;
            stationName.text = TextHelper.GetTextFromChild(textHolder, name);

        }
        //Debug.Log($"stationName.text:{stationName.text}");
       
    }

    public string GetStationName() 
    {
        //Debug.Log($"stationName.text:{stationName.text}");
        return stationName.text;
    }

}
