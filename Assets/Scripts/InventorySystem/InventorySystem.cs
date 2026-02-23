using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public int[,] inventoryGrid;
    public int height;
    public int width;

    public GameObject invSquareGO;
    public GameObject[,] invSquareGOArray;
    public List<GameObject> addOnSpawn;
    public bool notMain = false;

    private void Awake()
    {
        inventoryGrid = new int[width, height];
        invSquareGOArray = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                invSquareGOArray[x,y] = Instantiate(invSquareGO, transform.position, transform.rotation, this.transform.GetChild(0));
                invSquareGOArray[x, y].GetComponent<RectTransform>().localPosition = new Vector3(x * 51, y * 51);
                invSquareGOArray[x, y].GetComponent<InventorySquare>().x = x;
                invSquareGOArray[x, y].GetComponent<InventorySquare>().y = y;
            }
        }
    }
    private void Start()
    {
        foreach(GameObject item in addOnSpawn)
        {
            AddItem(item);
        }
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y>= 0 && x < width && y < height)
        {
            inventoryGrid[x, y] = value;
            invSquareGOArray[x, y].GetComponent<InventorySquare>().value = value;
        }
    }
    public int GetValue(int x, int y)
    {
        return inventoryGrid[x, y];
    }

    public void AddItem(GameObject gameObject)
    {
        InventoryItem itemStats = gameObject.GetComponent<InventoryItem>();
        Vector2Int spawnPos = isInventoryFull(gameObject, true);
        if (spawnPos != new Vector2Int(100, 100))
        {
            GameObject item = Instantiate(gameObject, transform.position, transform.rotation, transform);
            item.GetComponent<InventoryItem>().curretPos = spawnPos;
            item.GetComponent<RectTransform>().localPosition = new Vector3((25.5f * (itemStats.width - 1)) + (spawnPos.x * 51), (25.5f * (itemStats.height - 1)) + (spawnPos.y * 51));
            item.transform.localScale = new Vector3(1, 1, 1);
            item.GetComponent<InventoryItem>().notMain = notMain;
            item.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }
    public Vector2Int isInventoryFull(GameObject gameObject, bool fillSlots)
    {
        InventoryItem itemStats = gameObject.GetComponent<InventoryItem>();
        Vector2Int spawnPos = new Vector2Int(100,100); //will never be this large so its basicly "null"
        bool canFit = false;
        for (int x = 0; x < width; x++)
        {
            if (canFit) { break; }
            for (int y = 0; y < height; y++)
            {
                if (canFit) { break; }

                int blockedSlots = 0;
                for (int ix = 0; ix < itemStats.width; ix++)
                {
                    if (blockedSlots > 0) { break; }
                    for (int iy = 0; iy < itemStats.height; iy++)
                    {
                        if (blockedSlots > 0) { break; }
                        if (x + ix < 0 || y + iy < 0 || x + ix >= inventoryGrid.GetLength(0) || y + iy >= inventoryGrid.GetLength(1))
                        {
                            blockedSlots++;
                            break;
                        }
                        if (GetValue(x + ix, y + iy) == 1)
                        {
                            blockedSlots++;
                            break;
                        }
                    }
                }
                if (blockedSlots == 0)
                {
                    canFit = true;
                    spawnPos = new Vector2Int(x, y);

                    if (fillSlots)
                    {
                        //Fill the slots
                        for (int ix = 0; ix < itemStats.width; ix++)
                        {
                            for (int iy = 0; iy < itemStats.height; iy++)
                            {
                                SetValue(x + ix, y + iy, 1);
                            }
                        }
                    }
                }
            }
        }
        return spawnPos;
    }
}
