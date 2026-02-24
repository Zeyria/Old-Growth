using UnityEngine;
using System.Collections.Generic;

public class DropMarker : MonoBehaviour
{
    public List<GameObject> drops;
    public GameObject dropInventory;
    private void Start()
    {
        MakeDropInventory();
    }
    void MakeDropInventory()
    {
        GameObject a = Instantiate(dropInventory);
        foreach (GameObject drop in drops)
        {
            a.transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().AddItem(drop);
        }
        a.transform.GetChild(0).GetChild(1).GetComponent<CloseDropWindow>().dropMarker = this.gameObject;
        Time.timeScale = 0;
    }
}
