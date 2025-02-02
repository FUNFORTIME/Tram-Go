using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassengerRateDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passengerRateText;
    private Tram tram;

    private void Start()
    {
        tram =Manager.instance.tram;
        StartCoroutine(ChangePassengerRate());
    }

    private IEnumerator ChangePassengerRate()
    {
        while (true)
        {
            passengerRateText.text = 
                ((int)((float)(tram.passengerList.Count+tram.passengerUnboarding.Count) / (float)tram.capacity*100)).ToString() + "%";
            yield return new WaitForSeconds(1);
        }
    }

}
