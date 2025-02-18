using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleDisplay : MonoBehaviour
{
    private int powerNum;
    private int brakeNum;

    [SerializeField] private List<Image> powerLights;
    [SerializeField] private List<Image> brakeLights;
    [SerializeField] private Image idleLight;
    [SerializeField] private GameObject EBHandlePrefab;
    [SerializeField] private GameObject BHandlePrefab;
    [SerializeField] private GameObject NHandlePrefab;
    [SerializeField] private GameObject PHandlePrefab;
    [SerializeField] private AudioSource toggleAudioSource;
    private Tram tram;

    void Start()
    {
        tram = Manager.instance.tram;
    }

    public void ChangeHandle(int _handle)
    {
        toggleAudioSource.volume = 0.5f;
        toggleAudioSource.Play();

        foreach (Image light in powerLights.Concat(brakeLights))
        {
            Color _color = light.color;
            light.color = new Color(_color.r, _color.g, _color.b, .5f);
        }

        Color _idleColor = idleLight.color;
        idleLight.color = new Color(_idleColor.r, _idleColor.g, _idleColor.b, 1f);

        if (_handle > 0)
        {
            for (int i = 0; i < _handle; i++)
            {
                Color _color = powerLights[i].color;
                powerLights[i].color = new Color(_color.r, _color.g, _color.b, 1f);
            }
        }
        else if(_handle < 0)
        {
            for (int i = 0; i < -_handle; i++)
            {
                Color _color = brakeLights[i].color;
                brakeLights[i].color = new Color(_color.r, _color.g, _color.b, 1f);
            }
        }
    }

    [ContextMenu("CreateHandle")]
    private void CreateHandle()
    {
        GameObject CreateGear(GameObject _prefab,string _text)
        {
            GameObject _obj = Instantiate(_prefab);
            _obj.transform.SetParent(this.transform);
            _obj.GetComponentInChildren<TextMeshProUGUI>().text=_text;
            return _obj;
        }

        powerLights.Clear();
        brakeLights.Clear();
        idleLight = null;

        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        powerNum = tram.powerHandles.Count;
        for (int i = powerNum-1; i >=0; i--)
            powerLights.Add(CreateGear(PHandlePrefab, "P" + (i + 1).ToString()).GetComponentInChildren<Image>());
        powerLights.Reverse();

        idleLight=CreateGear(NHandlePrefab, "N").GetComponentInChildren<Image>();

        brakeNum = tram.brakeHandles.Count;
        for (int i = 0; i < brakeNum-1; i++)
            brakeLights.Add(CreateGear(BHandlePrefab, "B" + (i + 1).ToString()).GetComponentInChildren<Image>());

        brakeLights.Add(CreateGear(EBHandlePrefab, "EB").GetComponentInChildren<Image>());
    }

}
