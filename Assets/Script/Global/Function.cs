using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VirtualTime
{
    public int h;public int m;public int s;

    public VirtualTime(int h, int m, int s)
    {
        this.h = h;
        this.m = m;
        this.s = s;
    }

    public static VirtualTime operator +(VirtualTime t1, VirtualTime t2)
    {
        int _s = t1.s + t2.s;
        int _m = t1.m + t2.m;
        int _h = t1.h + t2.h;
        if (_s >= 60)
        {
            _s -= 60;
            _m++;
        }
        if (_m >= 60)
        {
            _m -= 60;
            _h++;
        }

        return new VirtualTime(_h, _m, _s);
    }

    public static VirtualTime operator -(VirtualTime t1, VirtualTime t2)
    {
        int _s = t1.s - t2.s;
        int _m = t1.m - t2.m;
        int _h = t1.h - t2.h;
        if (_s < 0)
        {
            _s += 60;
            _m--;
        }
        if (_m < 0)
        {
            _m += 60;
            _h--;
        }

        return new VirtualTime(_h, _m, _s);
    }

    public override string ToString()
    {
        if (h == 0 && m == 0 && s == 0) return "--:--:--";
        return string.Format("{0:D2}", h) + ":" + string.Format("{0:D2}", m) + ":" + string.Format("{0:D2}", s);
    }

    public int ToInt() => s + m * 60 + h * 3600;

    public VirtualTime FromInt(int _s)
    {
        int _m = _s / 60;
        _s %= 60;
        int _h = _m / 60;
        _m %= 60;

        return new VirtualTime(_h, _m, _s);
    }

    public static VirtualTime CurrentTime() => new VirtualTime().FromInt((int)Time.timeSinceLevelLoad) +LevelInfo.instance.departureTime;

}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("Keys count is not equal to values count");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
[System.Serializable]
public class GameData
{
    public int xp;
    public float effectVolume;
    public float voiceVolume;
    public GameMode gameMode;
    public LanguageType language;
    public SerializableDictionary<string, bool> routeUnlock=null;
    public SerializableDictionary<string, bool> levelUnlock = null;
    public SerializableDictionary<string, int> levelHighScore = null;

    public GameData()
    {
        xp = 0;
        effectVolume = 0.5f;
        voiceVolume = 0.5f;
        gameMode = GameMode.normal;
        language = LanguageType.english;
        routeUnlock = new SerializableDictionary<string, bool>();
        levelUnlock = new SerializableDictionary<string, bool>();   
        levelHighScore = new SerializableDictionary<string, int>();
    }

    public GameData CompleteCheck()
    {
        if(routeUnlock==null)routeUnlock = new SerializableDictionary<string, bool>();
        if (levelUnlock == null) levelUnlock = new SerializableDictionary<string, bool>();
        if(levelHighScore==null)levelHighScore = new SerializableDictionary<string, int>();
        return this;
    }
}