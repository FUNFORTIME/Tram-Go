using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class UnlockConformDisplay : MonoBehaviour
{
    public static UnlockConformDisplay instance;

    [SerializeField]private TextMeshProUGUI conformText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject textBox;
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
                string.Format($"{TXT("UnlockConform1")} {route.xpToUnlock.ToString()}XP {TXT("UnlockConform2")} {TXT(route.routeName)} ?");//UnlockConform

            yesButton.interactable = true;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(routeDisplay.Unlock);
            yesButton.onClick.AddListener(Hide);
        }
        else
        {
            conformText.text= string.Format( $"{TXT("UnlockXPnotEnough")} {TXT(route.routeName)}");//UnlockXPnotEnough

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
                string.Format($"{TXT("UnlockConform1")} {level.xpToUnlock.ToString()}XP {TXT("UnlockConform2")} ?");//UnlockConform
            

            yesButton.interactable = true;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(levelDisplay.Unlock);
            yesButton.onClick.AddListener(Hide);
        }
        else
        {
            conformText.text = string.Format($"{TXT("UnlockXPnotEnough")}");//UnlockXPnotEnough
            yesButton.onClick.RemoveAllListeners();
            yesButton.interactable = false;
        }

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private string TXT(string name)
    {
        string ret = TextHelper.GetTextFromChild(textBox, name);
        return ret;
    }
}
