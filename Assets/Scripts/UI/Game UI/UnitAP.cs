using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAP : MonoBehaviour
{
    public int apOffset;
    public Sprite off;
    public Sprite on;
    void Update()
    {
        if(transform.GetComponentInParent<UnitStats>().actionPointCurrent <= apOffset)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = off;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = on;
        }

        if (transform.GetComponentInParent<UnitStats>().isEnemy)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
