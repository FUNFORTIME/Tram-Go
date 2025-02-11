using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteDisplay : MonoBehaviour
{
    public Route route;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI routeText;
    [SerializeField]private TextMeshProUGUI unlockText;

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        routeText.text = route.routeName;
        unlockText.text = route.unlock ? "" : route.xpToUnlock + "XP";

        button.onClick.RemoveAllListeners();
        if(route.unlock)
        {
            button.onClick.AddListener(()=>LevelManager.instance.Show(route));
        }
        else
        {
            button.onClick.AddListener(() => UnlockConformDisplay.instance.Show(this));
        }
    }

    public void Unlock()
    {
        SaveManager.instance.gameData.xp -= route.xpToUnlock;
        SaveManager.instance.gameData.routeUnlock[route.routeName] = true;
        SaveManager.instance.SaveGame();

        XPDisplay.instance.UpdateXPDisplay();
        route.unlock = true;
        UpdateDisplay();
    }

    private void OnValidate()
    {
        name = route.routeName;
        
    }

}
