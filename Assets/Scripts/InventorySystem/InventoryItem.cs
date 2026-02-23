using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int width;
    public int height;

    public Vector2Int curretPos;

    Vector3 offset;
    Vector3 originalPos;
    public GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public InventorySystem inventorySystem;
    Vector2Int gridOffset;
    public bool notMain = false;
    GraphicRaycaster mainRaycaster;

    void Start()
    {
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        m_Raycaster = transform.root.GetComponent<GraphicRaycaster>();
        if (notMain)
        {
            mainRaycaster = GameObject.Find("GameUI").GetComponent<GraphicRaycaster>();
        }
        else
        {
            if(GameObject.Find("MobDropCanvas") != null)
            {
                mainRaycaster = GameObject.Find("MobDropCanvas").GetComponent<GraphicRaycaster>();
            }
            if (GameObject.Find("MobDropCanvas(Clone)") != null)
            {
                mainRaycaster = GameObject.Find("MobDropCanvas(Clone)").GetComponent<GraphicRaycaster>();
            }
        }
        inventorySystem = transform.parent.gameObject.GetComponent<InventorySystem>();

        FillSlots(curretPos, 1);

        //Fetch the Event Trigger component from your GameObject
        EventTrigger trigger = GetComponent<EventTrigger>();
        //Create a new entry for the Event Trigger
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //Add a Drag type event to the Event Trigger
        entry.eventID = EventTriggerType.Drag;
        //call the OnDragDelegate function when the Event System detects dragging
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        //Add the trigger entry
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.BeginDrag;
        entry2.callback.AddListener((data) => { BeginDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.EndDrag;
        entry3.callback.AddListener((data) => { EndDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry3);
    }
    public void Update()
    {
        if(mainRaycaster == null)
        {
            if (GameObject.Find("MobDropCanvas") != null && !notMain)
            {
                mainRaycaster = GameObject.Find("MobDropCanvas").GetComponent<GraphicRaycaster>();
            }
            if (GameObject.Find("MobDropCanvas(Clone)") != null && !notMain)
            {
                mainRaycaster = GameObject.Find("MobDropCanvas(Clone)").GetComponent<GraphicRaycaster>();
            }
        }
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (notMain)
            {
                if (GameObject.Find("Inventory").GetComponent<InventorySystem>().isInventoryFull(this.gameObject, false) == new Vector2Int(100, 100)) { return; }
                GameObject.Find("Inventory").GetComponent<InventorySystem>().AddItem(this.gameObject);
                FillSlots(curretPos, 0);
                notMain = false;
                Destroy(this.gameObject);
            }
            else if(GameObject.Find("MobDropCanvas") != null)
            {
                if (GameObject.Find("MobDropCanvas").transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().isInventoryFull(this.gameObject, false) == new Vector2Int(100, 100)) { return; }
                GameObject.Find("MobDropCanvas").transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().AddItem(this.gameObject);
                FillSlots(curretPos, 0);
                notMain = false;
                Destroy(this.gameObject);
            }
            else if (GameObject.Find("MobDropCanvas(Clone)") != null)
            {
                if (GameObject.Find("MobDropCanvas(Clone)").transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().isInventoryFull(this.gameObject, false) == new Vector2Int(100, 100)) { return; }
                GameObject.Find("MobDropCanvas(Clone)").transform.GetChild(0).GetChild(0).GetComponent<InventorySystem>().AddItem(this.gameObject);
                FillSlots(curretPos, 0);
                notMain = false;
                Destroy(this.gameObject);
            }
        }
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            ActivateItem();
        }
    }
    public void BeginDragDelegate(PointerEventData data)
    {
        Vector3 point = Input.mousePosition;
        offset = point - gameObject.GetComponent<RectTransform>().position;
        originalPos = transform.localPosition;
        FillSlots(curretPos, 0); // Clear old pos

        //Gets original slot clicked
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        //if (isWorldSpace) { m_PointerEventData.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); }
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        if (mainRaycaster != null)
        {
            mainRaycaster.Raycast(m_PointerEventData, results);
        }

        gridOffset = new Vector2Int(0, 0);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == "InvSquare(Clone)")
            {
                InventorySquare square = result.gameObject.GetComponent<InventorySquare>();
                gridOffset = new Vector2Int(square.x - curretPos.x, square.y - curretPos.y);
                gridOffset = -gridOffset;
            }
        }

        //Debug.Log(gridOffset.ToString());
        transform.root.GetComponent<Canvas>().sortingOrder = 1;
    }
    public void OnDragDelegate(PointerEventData data)
    {
        Vector3 point = Input.mousePosition;
        gameObject.GetComponent<RectTransform>().position = point - offset;
    }
    public void EndDragDelegate(PointerEventData data)
    {
        transform.root.GetComponent<Canvas>().sortingOrder = 0;
        offset = Vector3.zero;

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
        //if (isWorldSpace) { m_PointerEventData.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); }

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        if (mainRaycaster != null)
        {
            mainRaycaster.Raycast(m_PointerEventData, results);
        }

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        int hitSquare = 0;
        bool differentGrid = false;
        foreach (RaycastResult result in results)
        {
            if(result.gameObject.name == "InvSquare(Clone)")
            {
                if(result.gameObject.transform.parent.parent != transform.parent)
                {
                    //Debug.Log("DifferentGrid");
                    inventorySystem = result.gameObject.transform.parent.parent.GetComponent<InventorySystem>();
                    differentGrid = true;
                }
                hitSquare++;
                InventorySquare square = result.gameObject.GetComponent<InventorySquare>();
                bool canFit = true;
                for (int x = 0; x < width; x++)
                {
                    if (!canFit) { break; }
                    for (int y = 0; y < height; y++)
                    {
                        if (!canFit) { break; }
                        if (square.x + x + gridOffset.x < 0 || square.y + y + gridOffset.y < 0 || square.x + x + gridOffset.x >= inventorySystem.inventoryGrid.GetLength(0) || square.y + y + gridOffset.y >= inventorySystem.inventoryGrid.GetLength(1))
                        {
                            canFit = false;
                            break;
                        }
                        if(inventorySystem.GetValue(square.x + x + gridOffset.x ,square.y + y + gridOffset.y) == 1)
                        {
                            canFit = false;
                        }
                    }
                }
                if (canFit)
                {
                    if (differentGrid)
                    {
                        notMain = false;
                        transform.SetParent(square.gameObject.transform.parent.parent);
                    }

                    FillSlots(new Vector2Int(square.x + gridOffset.x, square.y + gridOffset.y), 1); // Fill New Slots
                    //Debug.Log(square.ToString());
                    curretPos = new Vector2Int(square.x + gridOffset.x, square.y + gridOffset.y);

                    gameObject.GetComponent<RectTransform>().localPosition = new Vector3(result.gameObject.GetComponent<RectTransform>().localPosition.x + (25.5f * (width -1)) + (gridOffset.x * 51),
                        result.gameObject.GetComponent<RectTransform>().localPosition.y + (25.5f * (height-1)) + (gridOffset.y * 51));
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    //Debug.Log("Blocked");
                    gameObject.GetComponent<RectTransform>().localPosition = originalPos;
                    FillSlots(curretPos, 1); // Refill Old Slots
                    inventorySystem = transform.parent.gameObject.GetComponent<InventorySystem>(); //In case it was a different grid
                }
            }
        }
        if(hitSquare == 0)
        {
            //Debug.Log("Outside Grid");
            gameObject.GetComponent<RectTransform>().localPosition = originalPos;
        }
    }
    public void FillSlots(Vector2Int pos, int value)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                inventorySystem.SetValue(pos.x + x, pos.y + y, value);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public virtual void ActivateItem()
    {
        Debug.Log("Item Activated! This should probably be overridden.");
    }
}
