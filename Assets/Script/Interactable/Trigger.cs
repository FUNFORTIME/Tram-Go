using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private bool pause = true;

    public void CallTrigger()
    {
        panel.SetActive(true);

        UI.instance.pause.PauseOnly(pause);
    }
}
