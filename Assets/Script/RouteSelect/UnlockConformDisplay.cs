using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockConformDisplay : MonoBehaviour
{
    public static UnlockConformDisplay instance;

    [SerializeField]private TextMeshProUGUI conformText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(this.Hide);

        Hide();
    }

    public void ConformRoute(RouteDisplay routeDisplay)
    {
        Route route = routeDisplay.route;
        int _xp=SaveManager.instance.gameData.xp;

        gameObject.SetActive(true);

        if (_xp >= route.xpToUnlock)
        {
            conformText.text = 
                string.Format("Unlock {0} with {1}XP?", route.routeName, route.xpToUnlock.ToString());

            yesButton.interactable = true;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(routeDisplay.Unlock);
            yesButton.onClick.AddListener(Hide);
        }
        else
        {
            conformText.text= string.Format( "No enough XP to unlock {0}.",route.routeName);

            yesButton.onClick.RemoveAllListeners();
            yesButton.interactable = false;
        }
    }

    public void ConformLevel(LevelDisplay levelDisplay)
    {
        Level level = levelDisplay.level;
        int _xp = SaveManager.instance.gameData.xp;

        gameObject.SetActive(true);

        if (_xp >= level.xpToUnlock)
        {
            conformText.text =
                string.Format("Unlock {0} with {1}XP?", level.levelName, level.xpToUnlock.ToString());

            yesButton.interactable = true;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(levelDisplay.Unlock);
            yesButton.onClick.AddListener(Hide);
        }
        else
        {
            conformText.text = string.Format("No enough XP to unlock {0}.", level.levelName);

            yesButton.onClick.RemoveAllListeners();
            yesButton.interactable = false;
        }

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
