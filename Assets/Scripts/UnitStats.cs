using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public bool generated;
    public string unitName;
    public string unitClass;

    public int hpMax;
    [HideInInspector]
    public int hpCurrent;
    [HideInInspector]
    public int tempHp;
    [HideInInspector]
    public int tempStr;
    public int speed;
    public int attack;
    public int sightRange;
    public int size;
    [HideInInspector]
    public int personality = -1;

    public int actionPointMax;
    //[HideInInspector]
    public int actionPointCurrent;

    public ActionScriptableObject action1;
    public ActionScriptableObject action2;

    public Texture2D portrait;

    public bool isEnemy;
    public bool isCorpse = false;
    public bool spawnsCorpse = true;

    public List<ObjAndIntClass> dropList;
    public List<ObjAndIntClass> startGear;
}
