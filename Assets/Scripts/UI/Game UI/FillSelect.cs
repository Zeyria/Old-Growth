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
    public List<GameObject> uiButtons;
    private AStarPathfinding filled;
    public bool transfer;
    public GameObject transferTo;
    public bool notMain;
    void Awake()
    {
        if (notMain) { return; }
        uiButtons = new List<GameObject>();
        for (int i = 0; i <= availableUnits.Count - 1; i++)
        {
            GameObject unit = Instantiate(availableUnits[i].obj, transform.parent.parent.GetChild(2));
            availableUnits[i].obj = unit;
            GameObject a = Instantiate(selectUI, this.transform);

            UISet(unit, a);
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
    void UISet(GameObject unit, GameObject a)
    {
        int pers;
        int[] stats;
        if (unit.GetComponent<UnitStats>().personality == -1)
        {
            pers = Random.Range(0, PersonalityList.Personalites.Count);
            unit.GetComponent<UnitStats>().personality = pers;
            stats = PersonalityList.Personalites[pers].intArray;
        }
        else
        {
            pers = unit.GetComponent<UnitStats>().personality;
            stats = new int[] {0,0,0,0 };
        }

        if (!unit.GetComponent<UnitStats>().generated)
        {
            unit.GetComponent<UnitStats>().unitName = UnitNameList.firstNames[Random.Range(0, UnitNameList.firstNames.Length)] + " " + UnitNameList.lastNames[Random.Range(0, UnitNameList.lastNames.Length)];
            unit.GetComponent<UnitStats>().generated = true;

        }
        a.transform.GetChild(5).GetChild(1).GetComponent<TMP_Text>().text = PersonalityList.Personalites[pers].stri + " " + unit.GetComponent<UnitStats>().unitClass;
        a.transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = (stats[0] + unit.GetComponent<UnitStats>().hpMax).ToString();
        a.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = (stats[1] + unit.GetComponent<UnitStats>().speed).ToString();
        a.transform.GetChild(3).GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = (stats[2] + unit.GetComponent<UnitStats>().sightRange).ToString();
        a.transform.GetChild(3).GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = (stats[3] + unit.GetComponent<UnitStats>().attack).ToString();
        a.transform.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = unit.GetComponent<UnitStats>().unitName;
        a.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Sprite.Create(unit.GetComponent<UnitStats>().portrait, new Rect(0, 0, 32, 32), new Vector2(.5f, .5f));
        a.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(24, 24);
        a.GetComponent<UnitSelectButton>().transfer = transfer;
        a.GetComponent<UnitSelectButton>().transferTo = transferTo;

        if (!uiButtons.Contains(a))
        {
            uiButtons.Add(a);
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
            if(floorTiles.transform.parent.childCount > 2)
            {
                for (int x = 2; x < floorTiles.transform.parent.childCount; x++)
                {
                    availableUnits[x-2].obj = floorTiles.transform.parent.GetChild(x).gameObject;
                    floorTiles.transform.parent.GetChild(x).GetComponent<UnitStats>().generated = true;
                }
                Debug.Log(transform.childCount);
                for (int i = 0; i < transform.childCount; i++)
                {
                    if(i +2 >= floorTiles.transform.parent.childCount) { continue; }
                    UISet(floorTiles.transform.parent.GetChild(i + 2).gameObject, transform.GetChild(i).gameObject);
                }
                for (int i = 0; i < transform.parent.parent.GetChild(2).childCount; i++)
                {
                    Destroy(transform.parent.parent.GetChild(2).GetChild(0).gameObject);
                }
                int c = floorTiles.transform.parent.childCount;
                for (int x = 2; x < c; x++)
                {
                    floorTiles.transform.parent.GetChild(2).SetParent(transform.parent.parent.GetChild(2));
                }
            }
            /*
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
            */
            skipframe = !skipframe;
            return;
        }
        /* not sure what this did?
        if(this.transform.childCount != availableUnits.Count)
        {
            for (int i = 0; i <= transform.childCount - 1; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            Awake();
        }
        */
    }
}
