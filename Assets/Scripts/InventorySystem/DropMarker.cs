using UnityEngine;
using System.Collections.Generic;

public class DropMarker : MonoBehaviour
{
    public List<GameObject> drops;
    public GameObject dropInventory;
    UnitControler unitCon;
    bool hasSpawned;
    private void Start()
    {
        hasSpawned = false;
        unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
    }
    private void Update()
    {
        CheckForPlayerUnit();
    }
    void MakeDropInventory()
    {
        hasSpawned = true;
        GameObject a = Instantiate(dropInventory);
        foreach (GameObject drop in drops)
        {
            a.transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().AddItem(drop);
        }
        a.transform.GetChild(0).GetChild(1).GetComponent<CloseDropWindow>().dropMarker = this.gameObject;
        Time.timeScale = 0;
    }
    void CheckForPlayerUnit()
    {
        if (hasSpawned) { return; }
        GameObject unit = unitCon.GetUnitAtTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).y)));
        if(unit == null || unit.GetComponent<UnitStats>().isEnemy || unit.GetComponent<UnitStats>().isCorpse)
        {
            unit = unitCon.GetUnitAtTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).x + 1), Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).y)));
        }
        if (unit == null || unit.GetComponent<UnitStats>().isEnemy || unit.GetComponent<UnitStats>().isCorpse)
        {
            unit = unitCon.GetUnitAtTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).x - 1), Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).y)));
        }
        if (unit == null || unit.GetComponent<UnitStats>().isEnemy || unit.GetComponent<UnitStats>().isCorpse)
        {
            unit = unitCon.GetUnitAtTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).y + 1)));
        }
        if (unit == null || unit.GetComponent<UnitStats>().isEnemy || unit.GetComponent<UnitStats>().isCorpse)
        {
            unit = unitCon.GetUnitAtTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(transform.position).y - 1)));
        }
        if (unit != null)
        {
            if(unit.GetComponent<UnitStats>().isEnemy == false && unit.GetComponent<UnitStats>().isCorpse == false)
            {
                Debug.Log("StandingOnTile");
                MakeDropInventory();
            }
        }
    }
}
