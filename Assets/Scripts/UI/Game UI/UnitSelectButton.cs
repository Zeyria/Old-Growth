using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class UnitSelectButton : MonoBehaviour, IPointerClickHandler
{
    public int amount;
    FillSelect fill;
    int i;
    GameObject unit;
    public static GameObject[] slots;
    public static GameObject[] slotsPortraits;
    public GameObject armyGrid;
    public static int[] slotTracker;
    int currentSlot; // 10 is unfilled
    public GameObject inventory;
    private void Start()
    {
        currentSlot = 10;
        slotTracker = new int[4];
        slots = new GameObject[4];
        slotsPortraits = new GameObject[4];
        amount = 1;
        fill = transform.parent.GetComponent<FillSelect>();
        i = transform.GetSiblingIndex();
        unit = fill.availableUnits[i].obj;
        slots[0] = (GameObject.Find("Slot 1"));
        slots[1] = (GameObject.Find("Slot 2"));
        slots[2] = (GameObject.Find("Slot 3"));
        slots[3] = (GameObject.Find("Slot 4"));
        slotsPortraits[0] = (GameObject.Find("Slot 1 Portrait"));
        slotsPortraits[1] = (GameObject.Find("Slot 2 Portrait"));
        slotsPortraits[2] = (GameObject.Find("Slot 3 Portrait"));
        slotsPortraits[3] = (GameObject.Find("Slot 4 Portrait"));
        slotTracker[0] = 0;
        slotTracker[1] = 0;
        slotTracker[2] = 0;
        slotTracker[3] = 0;
        armyGrid = GameObject.Find("ArmyGrid");
        inventory = GameObject.Find("Inventory");
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {

        if (currentSlot != 10)
        {
            slotsPortraits[currentSlot].GetComponent<EmbarkPortrait>().ClearSlot();
            currentSlot = 10;

            for (int i = 0; i < unit.GetComponent<UnitStats>().startGear.Count; i++)
            {
                for (int x = 0; x < unit.GetComponent<UnitStats>().startGear[i].nt; x++)
                {
                    inventory.GetComponent<InventorySystem>().RemoveItem(unit.GetComponent<UnitStats>().startGear[i].obj);
                }
            }
            SetStartingItems();
            return;
        }
        if (amount > 0)
        {
            GameObject activeSlot = null;
            GameObject activeSlotPortrait = null;
            if (slotTracker[0] == 0)
            {
                activeSlot = slots[0];
                activeSlotPortrait = slotsPortraits[0];
                slotTracker[0] = 1;
                currentSlot = 0;
            }
            else if (slotTracker[1] == 0)
            {
                activeSlot = slots[1];
                activeSlotPortrait = slotsPortraits[1];
                slotTracker[1] = 1;
                currentSlot = 1;
            }
            else if (slotTracker[2] == 0)
            {
                activeSlot = slots[2];
                activeSlotPortrait = slotsPortraits[2];
                slotTracker[2] = 1;
                currentSlot = 2;
            }
            else if (slotTracker[3] == 0)
            {
                activeSlot = slots[3];
                activeSlotPortrait = slotsPortraits[3];
                slotTracker[3] = 1;
                currentSlot = 3;
            }

            if (activeSlot != null)
            {
                amount -= 1;
                GameObject gameObject = Instantiate(unit, activeSlot.transform.position + (new Vector3(.5f, 0) * (unit.GetComponent<UnitStats>().size - 1)), armyGrid.transform.rotation, armyGrid.transform.GetChild(0));

                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .6f);
                gameObject.name = transform.GetChild(4).transform.GetChild(1).GetComponent<TMP_Text>().text;
                this.transform.GetChild(6).gameObject.SetActive(true);

                gameObject.GetComponent<UnitStats>().hpMax = int.Parse(transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text);
                gameObject.GetComponent<UnitStats>().speed = int.Parse(transform.GetChild(3).transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>().text);
                gameObject.GetComponent<UnitStats>().sightRange = int.Parse(transform.GetChild(3).transform.GetChild(2).transform.GetChild(1).GetComponent<TMP_Text>().text);
                gameObject.GetComponent<UnitStats>().attack = int.Parse(transform.GetChild(3).transform.GetChild(3).transform.GetChild(1).GetComponent<TMP_Text>().text);

                gameObject.GetComponent<UnitStats>().unitName = transform.GetChild(4).transform.GetChild(1).GetComponent<TMP_Text>().text;
                gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                gameObject.transform.GetComponent<UnitStats>().isEnemy = false;
                activeSlotPortrait.GetComponent<RawImage>().texture = gameObject.GetComponent<UnitStats>().portrait;
                activeSlotPortrait.GetComponent<RawImage>().color = Color.white;
                //inv
                for (int i = 0; i < gameObject.GetComponent<UnitStats>().startGear.Count; i++)
                {
                    for (int x = 0; x < gameObject.GetComponent<UnitStats>().startGear[i].nt; x++)
                    {
                        inventory.GetComponent<InventorySystem>().AddItem(gameObject.GetComponent<UnitStats>().startGear[i].obj);
                    }
                }
                SetStartingItems();
            }
        }
    }
    public void SetStartingItems()
    {
        InventoryTownToArena.startingItems.Clear();
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            if (inventory.transform.GetChild(i).name == "Food(Clone)") { InventoryTownToArena.startingItems.Add(1); }
            else if (inventory.transform.GetChild(i).name == "Bandages(Clone)") { InventoryTownToArena.startingItems.Add(2); }
            else if (inventory.transform.GetChild(i).name == "Potion(Clone)") { InventoryTownToArena.startingItems.Add(3); }
        }
    }
}
