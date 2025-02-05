using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool pause = false;

    private void Start()
    {
        pause = false;
        ChangePauseDisplay();
    }

    public void ChangePause()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        ChangePauseDisplay();
    }

    private void ChangePauseDisplay()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).gameObject.SetActive(pause);
        }
    }
}
