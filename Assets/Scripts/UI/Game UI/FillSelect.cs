using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class FillSelect : MonoBehaviour
{
    public List<ObjAndIntClass> availableUnits;
    public GameObject selectUI;
    public GameObject selectCursor;
    public Tilemap floorTiles;
    bool skipframe = true;
    private List<GameObject> uiButtons;
    private AStarPathfinding filled;
    public Tile halfTile;
    void Awake()
    {
        uiButtons = new List<GameObject>();
        for (int i = 0; i <= availableUnits.Count - 1; i++)
        {
            GameObject unit = availableUnits[i].obj;
            unit.GetComponent<UnitStats>().unitName = UnitNameList.firstNames[Random.Range(0, UnitNameList.firstNames.Length)] + " " + UnitNameList.lastNames[Random.Range(0, UnitNameList.lastNames.Length)];
            GameObject a = Instantiate(selectUI, this.transform);
            a.GetComponent<RectTransform>().localPosition -= new Vector3(0, i * 225);
            a.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Sprite.Create(unit.GetComponent<UnitStats>().portrait, new Rect(0,0, 32, 32), new Vector2(.5f,.5f));
            a.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
            //This is so ugly and probably slow but it only runs a few times in a small scene so its fine
            int pers = Random.Range(0, PersonalityList.Personalites.Count);
            a.transform.GetChild(5).GetChild(1).GetComponent<TMP_Text>().text = "Personality: " + PersonalityList.Personalites[pers].stri;
            int[] stats = PersonalityList.Personalites[pers].intArray;

            a.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = (stats[0] + unit.GetComponent<UnitStats>().hpMax).ToString();
            a.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = (stats[1] + unit.GetComponent<UnitStats>().speed).ToString();
            a.transform.GetChild(3).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = (stats[2] + unit.GetComponent<UnitStats>().sightRange).ToString();
            a.transform.GetChild(3).GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = (stats[3] + unit.GetComponent<UnitStats>().attack).ToString();
            a.transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = unit.GetComponent<UnitStats>().unitName;

            uiButtons.Add(a);
        }
        filled = new AStarPathfinding(8,8);
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                //Debug.Log(x + " " + y);
                filled.grid[x, y].isWalkable = true;
            }
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Town"))
        {
            return;
        }
        if (floorTiles == null)
        {
            floorTiles = GameObject.Find("ArmyGrid").transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
            skipframe = true;
        }
        if (skipframe)
        {
            for (int i = 1; i <= DontDestroyOnLoadManager._ddolObjects.Count - 1; i++)
            {
                if(DontDestroyOnLoadManager._ddolObjects[i - 1] != null)
                {
                    if (DontDestroyOnLoadManager._ddolObjects[i - 1].name == "ArmyGrid")
                    {
                        floorTiles = DontDestroyOnLoadManager._ddolObjects[i - 1].transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
                    }
                }
            }
            for (int x = 0; x <= floorTiles.transform.parent.childCount - 1; x++)
            {
                for (int i = 0; i <= availableUnits.Count - 1; i++)
                {
                    if (floorTiles.transform.parent.GetChild(x).name == gameObject.transform.GetChild(x).transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text)
                    {
                        uiButtons[i].GetComponent<UnitSelectButton>().amount -= 1;
                    }
                }
            }
            skipframe = !skipframe;
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectCursor.transform.childCount > 0)
            {
                Destroy(selectCursor.transform.GetChild(0).gameObject);
            }
            else
            {
                int layerObject = 1;
                Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);

                if (hit.collider != null)
                {
                    for (int x = 0; x <= availableUnits.Count - 1; x++)
                    {
                        if(hit.collider.gameObject.name == gameObject.transform.GetChild(x).transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text)
                        {
                            transform.GetChild(x).GetComponent<UnitSelectButton>().amount += 1;
                        }
                    }
                        Destroy(hit.collider.gameObject);
                    filled.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x + 7), Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y + 6)].isWalkable = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x + 7) + " " + Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y + 6));
            if (selectCursor.transform.childCount > 0 && filled.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x + 7),Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y + 6)].isWalkable)
            {
                filled.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x + 7), Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y + 6)].isWalkable = false;
                selectCursor.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                selectCursor.transform.GetChild(0).GetComponent<UnitStats>().isEnemy = false;

                if( floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y))) == halfTile)
                {
                    selectCursor.transform.GetChild(0).transform.position -= new Vector3(0, .25f, 0);
                }
                selectCursor.transform.GetChild(0).SetParent(floorTiles.transform.parent);
            }
        }
    }
}
