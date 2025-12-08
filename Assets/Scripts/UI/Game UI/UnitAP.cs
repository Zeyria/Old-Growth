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
        //Red if enemy, Green if not
        if (transform.GetComponentInParent<UnitStats>().isEnemy)
        {

            gameObject.GetComponent<SpriteRenderer>().color = new Color32(231, 26, 26, 255);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        //Fog Check
        if (transform.parent.parent.GetComponent<SpriteRenderer>().enabled)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
