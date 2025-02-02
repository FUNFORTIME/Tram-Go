using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public Tram tram;
    public Transform frontCheck;

    public XPSystem XPSystem;
    public DescriptionDisplay descriptionController;
    public ReverseHandleDisplay reverseHandleController;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
