using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseLogic : MonoBehaviour
{
    public int roundLifeSpan;
    private int rounds = 0;
    public bool makeWalkableAfterDeath;
    public void RoundTick()
    {
        rounds++;
        //Debug.Log("RoundTick" + rounds);
        if (rounds >= roundLifeSpan)
        {
            if (makeWalkableAfterDeath)
            {
                GameObject.Find("Controller").GetComponent<UnitControler>().pathfinding.grid[(int)ComFunc.WorldToTileSpace(transform.position).x, (int)ComFunc.WorldToTileSpace(transform.position).y].isWalkable = true;
            }
            Destroy(this.gameObject);
        }
    }
}
