using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarNode : IHeapItem<AStarNode>
{
   // private Tilemap tilemap;

    public int x;
    public int y;

    public bool isWalkable;

    public int gCost;
    public int hCost;
    public int fCost;
    int heapIndex;

    public AStarNode cameFromNode;

    public AStarNode(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = false;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(AStarNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
