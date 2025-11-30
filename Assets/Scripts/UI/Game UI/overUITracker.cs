using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class overUITracker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UnitControler unitCon;
    void Start()
    {
        unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("overUI");
        unitCon.overUI = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        unitCon.overUI = false;
    }
}
