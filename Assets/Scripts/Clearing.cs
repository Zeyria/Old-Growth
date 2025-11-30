using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Clearing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MapClass mapSettings;
    public Sprite small;
    public Sprite medium;
    public Sprite big;
    public bool isActiveMap;
    public Button yourButton;
    public GameObject graphic;
    private string description;
    private void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        description = "This is a ";

        mapSettings = new MapClass(MapClass.Enum.MapSizeSmall);
        int rng = Random.Range(0, 3);
        if (rng == 0) 
        { 
            mapSettings.MapSize = MapClass.Enum.MapSizeSmall;
            graphic.GetComponent<Image>().sprite = small;
            description += "<color=red>small</color> clearing with <color=red>unknown</color> hostiles.";
        }
        if (rng == 1) 
        { 
            mapSettings.MapSize = MapClass.Enum.MapSizeMedium;
            graphic.GetComponent<Image>().sprite = medium;
            description += "<color=red>medium</color> clearing with <color=red>unknown</color> hostiles.";
        }
        if (rng == 2) 
        { 
            mapSettings.MapSize = MapClass.Enum.MapSizeBig;
            graphic.GetComponent<Image>().sprite = big;
            description += "<color=red>large</color> clearing with <color=red>unknown</color> hostiles.";
        }
        transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = description;
    }
    void TaskOnClick()
    {
        bool cashedBool = isActiveMap;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.parent.GetChild(i).GetComponent<Clearing>().isActiveMap = false;
            transform.parent.GetChild(i).GetComponent<Clearing>().MapSettingsChanged();
        }
        if (!cashedBool)
        {
            isActiveMap = true;
        }
        if (isActiveMap)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        MapSettingsChanged();
        //Debug.Log(ArenaSettings.activeMapSettings.MapSize);
    }
    public void MapSettingsChanged()
    {
        if (isActiveMap)
        {
            ArenaSettings.activeMapSettings = mapSettings;
            ArenaSettings.hasActiveMap = true;
            this.GetComponent<Image>().enabled = true;
        }
        else
        {
            this.GetComponent<Image>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActiveMap)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
