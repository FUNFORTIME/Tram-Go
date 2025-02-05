using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Route 
{
    public bool unlock;
    public Sprite sprite;
    public string routeName;
    public int xpToUnlock;
    public List<Level> levelList;

    public Route(bool unlock=false, Sprite sprite=null, string routeName="Route", int xpToUnlock=10, List<Level> levelList=null)
    {
        this.unlock = unlock;
        this.sprite = sprite;
        this.routeName = routeName;
        this.xpToUnlock = xpToUnlock;
        this.levelList = levelList;
    }
}
