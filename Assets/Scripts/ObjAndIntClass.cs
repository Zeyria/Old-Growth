using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjAndIntClass
{
    public GameObject obj;
    public int nt;

    public ObjAndIntClass(GameObject obj, int nt)
    {
        this.obj = obj;
        this.nt = nt;
    }
}
