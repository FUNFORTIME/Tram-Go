using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RouteManager : MonoBehaviour,ISaveManager
{

    public static RouteManager instance;
    public UnlockConformDisplay unlockConformDisplay;
    public int xp;

    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private List<Route> routeList;
    [SerializeField] private GameObject routeDisplayPrefab;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void UpdateDisplay()
    {
        xpText.text = xp + "XP";
    }

    public void LoadData(GameData _gameData)
    {
        xp = _gameData.xp;
        for (int i = 0; i < transform.childCount; i++)
        {
            RouteDisplay _route = transform.GetChild(i).GetComponent<RouteDisplay>();
            //_route.unlock = i< _gameData.routeUnlock.Count ? _gameData.routeUnlock[i] : false;
            _route.UpdateDisplay();
        }

        UpdateDisplay();
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.xp = xp;
        //_gameData.routeUnlock.Capacity = transform.childCount;

        for (int i = 0; i < transform.childCount; i++)
        {
            RouteDisplay _route = transform.GetChild(i).GetComponent<RouteDisplay>();
            //_gameData.routeUnlock[i] = _route.unlock;
        }
    }

    private void OnValidate()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        foreach (Route _route in routeList)
        {
            RouteDisplay _routeDisplay = Instantiate(routeDisplayPrefab).GetComponent<RouteDisplay>();
            _routeDisplay.route = _route;
            _routeDisplay.UpdateDisplay();
        }
    }
}
