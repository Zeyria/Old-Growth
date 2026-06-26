using UnityEngine;
using System.Collections.Generic;

public class InventoryTownToArena : MonoBehaviour
{
    public static List<int> startingItems;
    public bool load;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;

    private void Start()
    {
        if (!load)
        {
            startingItems = new List<int>();
        }
        else
        {
            foreach(int itemID in startingItems)
            {
                if (itemID == 1) { this.gameObject.GetComponent<InventorySystem>().AddItem(item1); }
                else if (itemID == 2) { this.gameObject.GetComponent<InventorySystem>().AddItem(item2); }
                else if (itemID == 3) { this.gameObject.GetComponent<InventorySystem>().AddItem(item3); }
                else if (itemID == 4) { this.gameObject.GetComponent<InventorySystem>().AddItem(item4); }
            }
        }
    }
}
