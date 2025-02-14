using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private List<Route> routes = new List<Route>();
    [SerializeField] private GameObject routeDisplayPrefab;

    private void Start()
    {
        gameData = SaveManager.instance.gameData;

        foreach(RouteDisplay _routeDisplay in GetComponentsInChildren<RouteDisplay>())
        {
            Route route = _routeDisplay.route;

            if (!gameData.routeUnlock.ContainsKey(route.routeName))
                gameData.routeUnlock.Add(route.routeName, route.xpToUnlock==0);

            _routeDisplay.route.unlock = gameData.routeUnlock[route.routeName];
            _routeDisplay.UpdateDisplay();
        }

        SaveManager.instance.gameData = gameData;
        SaveManager.instance.SaveGame();
    }

    [ContextMenu("GenerateRouteDisplay")]
    private void GenerateRouteDisplay()
    {
        for (int i = 0; i < transform.childCount; i++) 
            DestroyImmediate(transform.GetChild(i).gameObject);

        foreach (Route route in routes)
        {
            GameObject _obj = Instantiate(routeDisplayPrefab,transform);
            RouteDisplay _routeDisplay = _obj.GetComponent<RouteDisplay>();
            _routeDisplay.route = route;
            _routeDisplay.UpdateDisplay();
        }
    }
}
