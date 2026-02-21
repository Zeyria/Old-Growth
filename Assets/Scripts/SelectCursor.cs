using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectCursor : MonoBehaviour
{
    Vector3 pos;
    public Tilemap floorTiles;
    GameObject selectCursor;
    bool skipframe = true;
    private void Start()
    {
        selectCursor = GameObject.Find("select");
        floorTiles = GameObject.Find("ArmyGrid").transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(floorTiles == null)
        {
            floorTiles = GameObject.Find("ArmyGrid").transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        }
        if (skipframe)
        {
            skipframe = !skipframe;
            return;
        }
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = ComFunc.WorldToGridSpace(pos.x, pos.y);
        Tile select = (Tile)floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(pos.y * 7 - .5f), Mathf.RoundToInt(pos.x * 7 - .5f)));
        if (select != null)
        {
            pos = new Vector2(Mathf.RoundToInt(pos.y * 7 - .5f), Mathf.RoundToInt(pos.x * 7 - .5f));
            pos = pos / 7;
            pos = new Vector3((pos.y * -3.5f) + (pos.x * 3.5f), (pos.x * 1.75f + (pos.y * 1.75f)) + .25f, 0);
            if (Time.timeScale == 1)
            {
                selectCursor.transform.position = pos;
                selectCursor.SetActive(true);
            }
            else
            {
                selectCursor.SetActive(false);
            }
        }
        else
        {
            selectCursor.SetActive(false);
        }
    }
}
