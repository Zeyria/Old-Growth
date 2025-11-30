using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ActionClass
{
    public int targetingType; // 0- Enemy 1- Ally 2- Open
    public float damageMult;
    public bool stun;
    public bool summon;
    public int range;
    public int minRange;
    public int AP;
    public GameObject soundPrefab;
    public GameObject animationPrefab;
    public Sprite iconSprite;
    public bool AIUseWhenWander;
    public string description;

    public ActionClass(int targetingType, float damageMult, bool stun, int range,int AP, GameObject soundPrefab,
        GameObject animationPrefab, Sprite iconSprite, bool summon, bool AIUseWhenWander, string description)
    {
        this.damageMult = damageMult;
        this.targetingType = targetingType;
        this.stun = stun;
        this.range = range;
        this.AP = AP;
        this.soundPrefab = soundPrefab;
        this.animationPrefab = animationPrefab;
        this.iconSprite = iconSprite;
        this.summon = summon;
        this.AIUseWhenWander = AIUseWhenWander;
        this.description = description;
    }
}
