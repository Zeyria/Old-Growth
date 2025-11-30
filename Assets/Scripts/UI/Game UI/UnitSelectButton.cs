using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSelectButton : MonoBehaviour
{
    public int amount;
    GameObject selectCursor;
    Button button;
    FillSelect fill;
    int i;
    GameObject unit;
    private void Start()
    {
        i = transform.GetSiblingIndex();
        fill = transform.parent.GetComponent<FillSelect>();
        string[] temp = transform.GetChild(2).GetComponent<TMP_Text>().text.Split('x');
        int.TryParse(temp[1], out amount);
        selectCursor = GameObject.Find("select");
        unit = fill.availableUnits[i].obj;
        button = transform.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        if (transform.GetChild(2).GetComponent<TMP_Text>().text != "x0")
        {
            amount -= 1;
            GameObject gameObject = Instantiate(unit, selectCursor.transform.position + (new Vector3(.5f,0) * (unit.GetComponent<UnitStats>().size - 1)), selectCursor.transform.rotation, selectCursor.transform);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .6f);
            gameObject.name = transform.GetChild(4).transform.GetChild(1).GetComponent<TMP_Text>().text;

            gameObject.GetComponent<UnitStats>().hpMax = int.Parse(transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text);
            gameObject.GetComponent<UnitStats>().speed = int.Parse(transform.GetChild(3).transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>().text);
            gameObject.GetComponent<UnitStats>().sightRange = int.Parse(transform.GetChild(3).transform.GetChild(2).transform.GetChild(1).GetComponent<TMP_Text>().text);
            gameObject.GetComponent<UnitStats>().attack = int.Parse(transform.GetChild(3).transform.GetChild(3).transform.GetChild(1).GetComponent<TMP_Text>().text);

            gameObject.GetComponent<UnitStats>().unitName = transform.GetChild(4).transform.GetChild(1).GetComponent<TMP_Text>().text;
        }
    }
    private void Update()
    {
        transform.GetChild(2).GetComponent<TMP_Text>().text = "x" + amount;
    }
}
