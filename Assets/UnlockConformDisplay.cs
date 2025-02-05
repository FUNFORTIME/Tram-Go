using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockConformDisplay : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TextMeshProUGUI conformText;

    public void Show(RouteDisplay routeDisplay)
    {
        Route route = routeDisplay.route;
        gameObject.SetActive(true);

        if (RouteManager.instance.xp < route.xpToUnlock)
        {
            conformText.text = "No enough XP!";
            yesButton.interactable = false;
        }
        else
        {
            conformText.text = string.Format("Unlock {0} with {1}XP?", route.routeName, route.xpToUnlock);
            yesButton.interactable = true;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(routeDisplay.Unlock);
            yesButton.onClick.AddListener(Hide);

        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
