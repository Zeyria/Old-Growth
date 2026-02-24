using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public string unitName;
    public string unitClass;

    public int hpMax;
    //[HideInInspector]
    public int hpCurrent;
    public int speed;
    public int attack;
    public int sightRange;
    public int size;

    public int actionPointMax;
    //[HideInInspector]
    public int actionPointCurrent;

    public ActionClass action1;
    public ActionClass action2;

    public Texture2D portrait;

    public bool isEnemy;
    public bool isCorpse = false;
    public bool spawnsCorpse = true;

    public List<ObjAndIntClass> dropList;
}
