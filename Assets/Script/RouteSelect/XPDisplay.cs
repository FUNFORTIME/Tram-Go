using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPDisplay : MonoBehaviour
{
    public static XPDisplay instance;
    [SerializeField] private TextMeshProUGUI xpText;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        UpdateXPDisplay();
    }

    public void UpdateXPDisplay()
    {
        xpText.text = SaveManager.instance.gameData.xp.ToString()+"XP";
    }
}
