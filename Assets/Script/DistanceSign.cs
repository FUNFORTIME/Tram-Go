using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceSign : MonoBehaviour
{
    [SerializeField] private TextMeshPro signText;
    public int num;

    public void UpdateDisplay()
    {
        signText.text = num.ToString("D3");
        this.name= num.ToString("D3");
    }
}
