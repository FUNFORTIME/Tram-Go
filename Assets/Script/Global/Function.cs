using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public static VirtualTime CurrentTime() => new VirtualTime().FromInt((int)Time.time) +GlobalVar.instance.departureTime;

}

[System.Serializable]
public class GameData
{
    public int xp;
    public Dictionary<RouteDisplay,bool> routeUnlock;
    public Dictionary<RouteDisplay,List<LevelInfo>> levelInfos;

    public GameData()
    {
        this.xp = 0;
    }
}
