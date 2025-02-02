using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleController : MonoBehaviour
{
    public int handle = 0;
    public Tram tram;
    public int powerNum = 3;
    public int brakeNum = 6;

    public Image[] lights;

    void Start()
    {
        lights = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            handle--;
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            handle++;
        handle = Mathf.Clamp(handle, -brakeNum, powerNum);

        for (int i = 1; i < lights.Length; i++)
        {
            Color _color = lights[i].color;

            if (i <= brakeNum+1 && i >= handle + brakeNum+1)
                lights[i].color = new Color(_color.r, _color.g, _color.b, 1f);

            else if (i >= brakeNum+1 && i <= handle + brakeNum+1)
                lights[i].color = new Color(_color.r, _color.g, _color.b, 1f);

            else
                lights[i].color = new Color(_color.r, _color.g, _color.b, .5f);

        }

        tram.accelerationTarget = tram.accelerateAbility[handle+brakeNum];
    }
}
