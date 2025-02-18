using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenSwitcher : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    private void Awake()
    {
        SetFullScreen(Screen.fullScreen);
    }

    private void Start()
    {
        toggle.isOn = Screen.fullScreen;
    }

    public void ChangeFullscreen()
    {
        SetFullScreen(toggle.isOn);
    }

    public void SetFullScreen(bool _fullscreen)
    {
        if (_fullscreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            Screen.SetResolution(1200,800,false);
        }
    }
}
