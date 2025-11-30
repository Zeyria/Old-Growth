using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

public class AStarPathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public AStarNode[,] grid;
    private Heap<AStarNode> openList;
    private List<AStarNode> closedList;

    public AStarPathfinding(int width, int height)
    {
        grid = new AStarNode[width,height];
        openList = new Heap<AStarNode>(grid.GetLength(0) * grid.GetLength(1));
        closedList = new List<AStarNode>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new AStarNode(x, y);
                grid[x, y].isWalkable = false;
            }
        }
    }
    public List<AStarNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        AStarNode startNode = grid[startX, startY];
        AStarNode endNode = grid[endX, endY];
        openList.Clear();
        openList.Add(startNode);
        closedList.Clear();
        if (!endNode.isWalkable)
        {
            return null;
        }
        if (!startNode.isWalkable)
        {
            return null;
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                AStarNode pathNode = grid[x, y];
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            AStarNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //done
                //sw.Stop();
                //UnityEngine.Debug.Log("Path found: " + sw.ElapsedMilliseconds + " ms");
                return CalculatePath(endNode);
            }
            closedList.Add(currentNode);

            foreach (AStarNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
                if(tentGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentGCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //no path
        return null;

    }

    private List<AStarNode> CalculatePath(AStarNode endNode)
    {
        List<AStarNode> path = new List<AStarNode>();
        path.Add(endNode);
        AStarNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    public int CalculateDistance(AStarNode a, AStarNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private AStarNode GetLowestFCostNode(Heap<AStarNode> aStarNodes)
    {
        return aStarNodes.RemoveFirst();
    }

    private List<AStarNode> GetNeighbourList(AStarNode currentNode)
    {
        List<AStarNode> neighbourList = new List<AStarNode>();
        if(currentNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //Ignore Diagonals
            /*
            if(currentNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
            if(currentNode.y < grid.GetLength(0))
            {
                neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            */
        }

        if(currentNode.x + 1 < grid.GetLength(1))
        {
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            //Ignore Diagonals
            /*
            if(currentNode.y -1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            if(currentNode.y + 1 < grid.GetLength(0))
            {
                neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
            */
        }

        if (currentNode.y-1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if(currentNode.y+1 < grid.GetLength(0))
        {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }
        return neighbourList;
    }
    private AStarNode GetNode(int x, int y)
    {
        return grid[x, y];
    }
}
