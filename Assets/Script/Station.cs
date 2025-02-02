using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class Station : MonoBehaviour
{
    public float stationLength = 5f;
    [Range(0, 100)] public int population = 50;

    public List<Passenger> passengerList = new List<Passenger>();
    public Transform passengerParent;
    [SerializeField] private GameObject passengerMale;
    [SerializeField] private GameObject passengerFemale;
    [SerializeField] private TextMeshPro stationName;
    private Tram tram;

    public void SpawnPassenger(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            GameObject _prefab = Random.value < 0.5 ? passengerMale : passengerFemale;
            GameObject _passenger = Instantiate(_prefab, passengerParent);
            passengerList.Add(_passenger.GetComponent<Passenger>());

            _passenger.transform.localPosition = new Vector3(Random.Range(-stationLength / 2, stationLength / 2), 0);
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
    }

    void Update()
    {

    }
        private void OnValidate()
    {
        stationName.text = name;
    }
}
