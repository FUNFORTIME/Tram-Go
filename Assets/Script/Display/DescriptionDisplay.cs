using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionDisplay : MonoBehaviour
{
    [SerializeField]private Image textbox;
    [SerializeField] private TextMeshProUGUI description;

    private void Start()
    {
        textbox.enabled = false;
        description.text = "";
    }

    public IEnumerator ChangeDescription(string _description,Color _color,float _showDuration=1f)
    {
        textbox.color = _color;
        description.text = _description;
        textbox.enabled = true;

        yield return new WaitForSeconds(_showDuration);

        textbox.enabled=false;
        description.text = "";
    }
}
