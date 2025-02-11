using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public XPSystem XPSystem;
    public DescriptionDisplay descriptionController;
    public ReverseHandleDisplay reverseHandleController;
    public AheadInfoCheck aheadInfoCheck;
    public Pause pause;
    public ResultDisplay resultDisplay;
    public StopResultDisplay stopResultDisplay;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
