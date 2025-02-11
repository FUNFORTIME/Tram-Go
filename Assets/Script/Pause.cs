using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool pause = false;

    private void Start()
    {
    }

    public void ChangePause()
    {
        PauseOnly(!pause);
        ChangePauseDisplay();
    }
    public void ChangePause(bool _pause)
    {
        PauseOnly(_pause);
        ChangePauseDisplay();
    }


    public void PauseOnly(bool _pause)
    {
        pause = _pause;
        Time.timeScale = pause ? 0 : 1;
    }

    private void ChangePauseDisplay()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).gameObject.SetActive(pause);
        }
    }
}
