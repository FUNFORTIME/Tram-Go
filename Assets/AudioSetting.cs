using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private AudioSource effectTest;
    [SerializeField] private AudioSource voiceTest;

    void Start()
    {
        GameData gameData = SaveManager.instance.gameData;
        effectSlider.value = gameData.effectVolume;
        voiceSlider.value = gameData.voiceVolume;
    }

    public void SaveVolume()
    {
        SaveManager.instance.gameData.effectVolume = effectSlider.value;
        SaveManager.instance.gameData.voiceVolume = voiceSlider.value;
        SaveManager.instance.SaveGame();

        AudioManager.instance.SetVolume();
    }

    public void PlayEffectTest()
    {
        if(!effectTest.isPlaying)
            effectTest.Play();
    }
    public void PlayVoiceTest()
    {
        if (!voiceTest.isPlaying)
            voiceTest.Play();
    }
}
