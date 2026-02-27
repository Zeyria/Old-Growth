using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseDropWindow : MonoBehaviour, IPointerClickHandler
{
    public GameObject dropMarker;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(dropMarker != null)
        {
            Destroy(dropMarker);
        }
        if(GameObject.Find("Controller") != null)
        {
            GameObject.Find("Controller").GetComponent<UnitControler>().overUI = false;
        }
        Destroy(transform.root.gameObject);
    }
}
