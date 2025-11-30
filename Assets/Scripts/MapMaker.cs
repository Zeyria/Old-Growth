using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapMaker : MonoBehaviour
{
    [Header("Chunks")]
    [Space(10)]
    public List<GameObject> ones;
    public List<GameObject> twos;
    public List<GameObject> threes;
    public List<GameObject> fours;
    public GameObject startSquare;
    public GameObject objectiveSquare;
    [Space(20)]
    [Header("Other Stuff")]
    [Space(10)]
    public bool skipProcGen = false;
    public GameObject grid;
    public GameObject mapHolderF;
    public GameObject mapHolderE;
    public GameObject map;
    public string seed;
    public bool useRandomSeed;
    public UnitControler unitCon;
    public new GameObject camera;
    public Tile borderTile;
    public Tile borderTile2;
    public Tile borderTile3;

    private int randomFillPercent;
    private int spacePercentReq;
    private int attempts;
    private int localAttempts;
    private int globalAttempts;
    private int spawnInt = 0;
    public enum Enum
    {
       MapSizeSmall = 5,
       MapSizeMedium = 7,
       MapSizeBig = 9,
    };
    public Enum MapSize;
    private int[,] mapfill;

    private void Awake()
    {
        if (GameObject.Find("ArmyGrid") != null)
        {
            startSquare = GameObject.Find("ArmyGrid").transform.GetChild(0).gameObject;
            startSquare.transform.parent.gameObject.SetActive(false);
        }
        if (skipProcGen)
        {
            MapSize = (Enum)15;
            unitCon.pathfinding = new AStarPathfinding((int)MapSize * 7 + 10, (int)MapSize * 7 + 10);
        }
    }
    private void Start()
    {
        if(ArenaSettings.activeMapSettings != null)
        {
            MapSize = (Enum)ArenaSettings.activeMapSettings.MapSize;
        }
        Time.timeScale = 1;
        Application.runInBackground = true;
        globalAttempts = 0;

        DoItAll();
    }
    private void DoItAll()
    {
        if (skipProcGen)
        {
            BoundsInt boundsInt = mapHolderF.transform.GetComponent<Tilemap>().cellBounds;
            foreach (var tile in boundsInt.allPositionsWithin)
            {
                if (mapHolderF.transform.GetComponent<Tilemap>().GetTile(tile) != null)
                {
                    //Debug.Log(tile.x +" "+ tile.y);
                    unitCon.pathfinding.grid[tile.x, tile.y].isWalkable = true;
                    mapHolderF.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                    mapHolderF.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
                }
                else
                {

                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile);
                    }
                    else if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile2);
                    }
                    else
                    {
                        mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile3);
                    }
                    unitCon.pathfinding.grid[tile.x, tile.y].isWalkable = false;
                    mapHolderF.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                    mapHolderF.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
                }
            }
            BoundsInt boundsInt2 = mapHolderE.transform.GetComponent<Tilemap>().cellBounds;
            foreach (var tile in boundsInt2.allPositionsWithin)
            {
                if (mapHolderE.transform.GetComponent<Tilemap>().GetTile(tile) != null)
                {
                    mapHolderE.transform.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                    mapHolderE.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
                }
            }
        }
        //True number of attempts is much higher, but this does stop a stack overflow
        else if (globalAttempts < 3)
        {
            attempts = 0;
            localAttempts = 0;
            spacePercentReq = 3 * (int)MapSize;
            randomFillPercent = (int)MapSize * 2;
            ClearMap();
            MakeMap();
            globalAttempts++;
        }

    }
    private void Update()
    {
        /* For Testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoItAll();
        }
        */
    }
    void ClearMap()
    {
        spawnInt = 0;
        mapHolderE.GetComponent<Tilemap>().ClearAllTiles();
        mapHolderF.GetComponent<Tilemap>().ClearAllTiles();
        unitCon.pathfinding = new AStarPathfinding((int)MapSize * 7 + 10, (int)MapSize * 7 + 10);
        List<GameObject> destroyObjects = new List<GameObject>();
        for (int i = 2; i < map.transform.childCount; i++)
        {
            destroyObjects.Add(map.transform.GetChild(i).gameObject);
        }
        foreach(GameObject gameobject in destroyObjects)
        {
            Destroy(gameobject);
            spawnInt++;
        }

    }
    void MakeMap()
    {
        string tempseed;
        if (useRandomSeed)
        {
            tempseed = DateTime.Now.ToString() + attempts.ToString();
        }
        else
        {
            tempseed = seed + attempts.ToString();
        }
        System.Random prng = new System.Random(tempseed.GetHashCode());

        mapfill = new int[(int)MapSize+1, (int)MapSize+1];
        for (int x = 0; x < (int)MapSize+1; x++)
        {
            for (int y = 0; y < (int)MapSize+1; y++)
            {
                mapfill[x, y] = 1;
            }
        }
        for (int x = 0; x < (int)MapSize; x++)
        {
            for (int y = 0; y < (int)MapSize; y++)
            {
                mapfill[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;

            }
        }
        int[,] tempMapFill;
        tempMapFill = mapfill;
        for (int i = 0; i < 5; i++)
        {
            for (int x = 0; x < (int)MapSize; x++)
            {
                for (int y = 0; y < (int)MapSize; y++)
                {
                    int nFillCount = FillCount(x, y, tempMapFill);

                    if (nFillCount > 4)
                    {
                        mapfill[x, y] = 1;
                    }
                    else if (nFillCount < 4)
                    {
                        mapfill[x, y] = 0;
                    }
                }
            }
            tempMapFill = mapfill;
        }

        int totalSpace = 0;
        for (int x = 0; x < (int)MapSize; x++)
        {
            for (int y = 0; y < (int)MapSize; y++)
            {
                if (mapfill[x, y] == 0)
                {
                    totalSpace++;
                }
            }
        }
        //Debug.Log("totalSpace% " + totalSpace / (int)MapSize * (int)MapSize);
        if (totalSpace / (int)MapSize * (int)MapSize < spacePercentReq && attempts < 100)
        {
            attempts++;
            ClearMap();
            MakeMap();
            return;
        }
        float distance00 = 9999;
        float distance01 = 9999;
        Vector2 spawnPoint = Vector2.zero;
        Vector2 spawnPoint2 = Vector2.zero;
        for (int x = 0; x < (int)MapSize; x++)
        {
            for (int y = 0; y < (int)MapSize; y++)
            {
                if (mapfill[x, y] == 1 && FillCount(x, y, mapfill) < 7)
                {
                    float distanceCalc = MathF.Sqrt(Mathf.Pow(0 - x, 2) + Mathf.Pow(0 -y, 2));
                    float distanceCalc2 = MathF.Sqrt(Mathf.Pow(10 - x, 2) + Mathf.Pow(10 -y, 2));
                    if ((distanceCalc < distance00) || distance00 == 9999)
                    {
                        distance00 = distanceCalc;
                        spawnPoint = new Vector2(x, y);
                    }
                    if ((distanceCalc2 < distance01) || distance01 == 9999)
                    {
                        distance01 = distanceCalc2;
                        spawnPoint2 = new Vector2(x, y);
                    }
                }
            }
        }
        GameObject Startchunk = Instantiate(startSquare, ComFunc.GridToWorldSpace(spawnPoint.x, spawnPoint.y), this.transform.rotation, grid.transform).gameObject;
        camera.transform.position = ComFunc.GridToWorldSpace(spawnPoint.x, spawnPoint.y) + new Vector3(0, 4f);
        Combine(Mathf.RoundToInt(spawnPoint.x), Mathf.RoundToInt(spawnPoint.y), Startchunk);
        GameObject objectiveChunk = Instantiate(objectiveSquare, ComFunc.GridToWorldSpace(spawnPoint2.x, spawnPoint2.y), this.transform.rotation, grid.transform).gameObject;
        Combine(Mathf.RoundToInt(spawnPoint2.x), Mathf.RoundToInt(spawnPoint2.y), objectiveChunk);
        for (int x = 0; x < (int)MapSize; x++)
        {
            for (int y = 0; y < (int)MapSize; y++)
            {
                int type = ChosePieceType(x, y);
                //Debug.Log("loop" + x + y + " " + type);
                if (type != 0)
                {
                    if(type == 1)
                    {
                        GameObject spawnChunk = ones[prng.Next(0, ones.Capacity)];
                        GameObject chunk = Instantiate(spawnChunk, ComFunc.GridToWorldSpace(x, y), this.transform.rotation, grid.transform).gameObject;
                        Combine(x, y, chunk);
                    }
                    if (type == 2)
                    {
                        GameObject spawnChunk = twos[prng.Next(0, twos.Capacity)];
                        GameObject chunk = Instantiate(spawnChunk, ComFunc.GridToWorldSpace(x, y), this.transform.rotation, grid.transform).gameObject;
                        Combine(x, y, chunk);
                    }
                    if (type == 3)
                    {
                        GameObject spawnChunk = threes[prng.Next(0, threes.Capacity)];
                        GameObject chunk = Instantiate(spawnChunk, ComFunc.GridToWorldSpace(x, y), this.transform.rotation, grid.transform).gameObject;
                        Combine(x, y, chunk);
                    }
                    if (type == 4)
                    {
                        GameObject spawnChunk = fours[prng.Next(0, fours.Capacity)];
                        GameObject chunk = Instantiate(spawnChunk, ComFunc.GridToWorldSpace(x, y), this.transform.rotation, grid.transform).gameObject;
                        Combine(x, y, chunk);
                    }
                }
            }
        }
        for (int c = 0; c < grid.transform.childCount; c++)
        {
            Destroy(grid.transform.GetChild(c).gameObject);
        }

        BoundsInt boundsInt = mapHolderF.transform.GetComponent<Tilemap>().cellBounds;
        foreach (var tile in boundsInt.allPositionsWithin)
        {
            if (mapHolderF.transform.GetComponent<Tilemap>().GetTile(tile) != null)
            {
                //Debug.Log(tile.x +" "+ tile.y);
                unitCon.pathfinding.grid[tile.x, tile.y].isWalkable = true;
                mapHolderF.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                mapHolderF.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
            }
            else
            {

                if(UnityEngine.Random.Range(0,2) == 0)
                {
                    mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile);
                }
                else if(UnityEngine.Random.Range(0, 2) == 0)
                {
                    mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile2);
                }
                else
                {
                    mapHolderF.GetComponent<Tilemap>().SetTile(new Vector3Int(tile.x, tile.y, 0), borderTile3);
                }
                unitCon.pathfinding.grid[tile.x, tile.y].isWalkable = false;
                mapHolderF.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                mapHolderF.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
            }
        }
        BoundsInt boundsInt2 = mapHolderE.transform.GetComponent<Tilemap>().cellBounds;
        foreach (var tile in boundsInt2.allPositionsWithin)
        {
            if (mapHolderE.transform.GetComponent<Tilemap>().GetTile(tile) != null)
            {
                mapHolderE.transform.GetComponent<Tilemap>().SetTileFlags(tile, TileFlags.None);
                mapHolderE.GetComponent<Tilemap>().SetColor(tile, unitCon.fog);
            }
        }
        //Start square also has 2 tilemaps on it, so + 2
        int spawners = map.transform.childCount - 2 - spawnInt - (startSquare.transform.childCount - 2);
        //Debug.Log(spawners);
        if(spawners < ((int)MapSize*(int)MapSize)/5)
        {
            seed = seed + 1;
            DoItAll();
        }

    }
    int FillCount(int x, int y, int[,] tempMapFill)
    {
        int fillCount = 0;
        for (int nX = x - 1; nX <= x + 1; nX++)
        {
            for (int nY = y - 1; nY <= y + 1; nY++)
            {
                if(nX >= 0 && nX < tempMapFill.Length && nY >= 0 && nY < tempMapFill.Length)
                {
                    if (nX != x || nY != y)
                    {
                        fillCount += tempMapFill[nX, nY];
                    }
                }
                else
                {
                    fillCount++;
                }
            }
        }
        return fillCount;
    }
    int ChosePieceType(int x, int y)
    {
        string tempseed = seed + localAttempts.ToString();
        System.Random prng = new System.Random(tempseed.GetHashCode());
        localAttempts++;

        if (mapfill[x,y] != 0)
        {
            //Debug.Log("Filled");
            return 0;
        }
        int randomRange = prng.Next(1, 6);
        //Debug.Log(randomRange);
        if (randomRange == 1)
        {
            mapfill[x, y] = 1;
            return 1;
        }
        if (randomRange == 2 && mapfill[x, y] == 0 && mapfill[x+1, y] == 0)
        {
            mapfill[x, y] = 1;
            if ((x >= 0) && (x < mapfill.Length)) { mapfill[x+1, y] = 1; }
            return 2;
        }
        else if (randomRange == 2)
        {
            return ChosePieceType(x, y);
        }
        if (randomRange == 3 && mapfill[x, y] == 0 && mapfill[x, y+1] == 0)
        {
            mapfill[x, y] = 1;
            if ((x >= 0) && (x < mapfill.Length)) { mapfill[x, y+1] = 1; }
            return 3;
        }
        else if (randomRange == 3)
        {
            return ChosePieceType(x, y);
        }
        if ((randomRange == 4 || randomRange == 5) && mapfill[x, y] == 0 && mapfill[x, y + 1] == 0 && mapfill[x + 1, y] == 0 && mapfill[x+1, y+1] == 0)
        {
            mapfill[x, y] = 1;
            if ((x >= 0) && (x < mapfill.Length)) { mapfill[x+1, y] = 1; }
            if ((x >= 0) && (x < mapfill.Length)) { mapfill[x+1, y+1] = 1; }
            if ((x >= 0) && (x < mapfill.Length)) { mapfill[x, y+1] = 1; }
            return 4;
        }
        else if (randomRange == 4 || randomRange == 5)
        {
            return ChosePieceType(x, y);
        }
        return 0;
    }
    void Combine(int x, int y, GameObject source)
    {
        BoundsInt boundsInt = source.transform.GetChild(0).GetComponent<Tilemap>().cellBounds;
        foreach (var tile in boundsInt.allPositionsWithin)
        {
            if(source.transform.GetChild(0).GetComponent<Tilemap>().GetTile(tile) != null)
            {
                mapHolderF.GetComponent<Tilemap>().SetTile(tile + ComFunc.GridToTileSpace(x + 1, y + 1), source.transform.GetChild(0).GetComponent<Tilemap>().GetTile(tile));
            }
            if (source.transform.GetChild(1).GetComponent<Tilemap>().GetTile(tile) != null)
            {
                mapHolderE.GetComponent<Tilemap>().SetTile(tile + ComFunc.GridToTileSpace(x + 1, y + 1), source.transform.GetChild(1).GetComponent<Tilemap>().GetTile(tile));
            }
        }
        for (int i = 2; i < source.transform.childCount; i=2)
        {
            if(source == startSquare)
            {
                DoItLater(source);
                break;
            }
            source.transform.GetChild(2).transform.position += ComFunc.GridToWorldSpace(1, 1);
            source.transform.GetChild(2).SetParent(map.transform);
        }

    }
    IEnumerator DoItLater(GameObject gameObject)
    {
        yield return new WaitForSeconds(.05f);
        gameObject.transform.GetChild(2).transform.position += ComFunc.GridToWorldSpace(1, 1);
        gameObject.transform.GetChild(2).SetParent(map.transform);
    }
}


