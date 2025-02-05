using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RouteDisplay : MonoBehaviour
{
    public Route route;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI routeNameText;
    [SerializeField] private TextMeshProUGUI unlockText;

    public void UpdateDisplay()
    {
        button.onClick.RemoveAllListeners();

        if (route.unlock)
            button.onClick.AddListener(this.SelectLevel);
        else
            button.onClick.AddListener(this.ConformingUnlock);

        image.color = new Color(1, 1, 1, route.unlock ? 1 : 0.2f);
        unlockText.text = route.unlock ? "" : route.xpToUnlock.ToString() + "XP";
    }

    private void SelectLevel()
    {

    }

    private void ConformingUnlock()
    {
        RouteManager.instance.unlockConformDisplay.Show(this);
    }

    public void Unlock()
    {
        RouteManager.instance.xp-=route.xpToUnlock;
        route.unlock = true;

        UpdateDisplay();
        RouteManager.instance.UpdateDisplay();
    }

    private void OnValidate()
    {
        routeNameText.text = route.routeName;
        this.name = route.routeName;

    }
}
