using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum cabinSFXType
{
    doorClose=5,
    doorOpen=6,
    motor=12,
    motorRise=13,
    compBegin=2,
    compEnd=3,
    compLoop=4,
    reduceSpeed=7,
    brake=1,
    brakeRelese=0,
    idle=8,

}
public enum sfxType
{
    airLow=0,
    airHigh=1,
    speedLimitConform=2,
    speedLimitUpdate=3,
    punish=4,
    handClap=5,
    getBronzeMedal=6,
    getSilverMedal=7,
    getGoldMedal=8,
}
public enum stationSFXType
{
    lowPopulation=2,
    mediumPopulation=1,
    highPopulation=0,
    doorCloseBell=3,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;
    public AudioSource[] otherSFX;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        SetVolume();
    }

    public void SetVolume()
    {
        float CalculateDB(float _value) => Mathf.Max(-80f, Mathf.Log10(_value)*10f);

        GameData gameData = SaveManager.instance.gameData;
        float _effectDB = CalculateDB(gameData.effectVolume);
        float _voiceDB = CalculateDB(gameData.voiceVolume);

        audioMixer.SetFloat("EffectVolume", _effectDB);
        audioMixer.SetFloat("VoiceVolume", _voiceDB);
    }

    public bool PlaySFX(AudioSource audioSource,float pitch=1f, float volume=1f)
    {
        audioSource.pitch = pitch > 0 ? pitch : 0;
        audioSource.volume = Mathf.Clamp01(volume);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        return !audioSource.isPlaying;
    }
    public bool PlaySFX(sfxType _sfxIndex, float pitch = 1f, float volume = 1f)
    {
        int _index = ((int)_sfxIndex);

        otherSFX[_index].pitch = pitch>0?pitch:0;
        otherSFX[_index].volume = Mathf.Clamp01(volume);

        if (!otherSFX[_index].isPlaying)
            otherSFX[_index].Play();

        return !otherSFX[_index].isPlaying;
    }

    public void StopSFX(AudioSource audioSource) => audioSource.Stop();
    public void StopSFX(sfxType _sfxIndex) => otherSFX[(int)_sfxIndex].Stop();
}
