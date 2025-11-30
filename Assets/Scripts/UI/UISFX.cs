using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISFX : MonoBehaviour, IPointerClickHandler
{
    public GameObject selectSound;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Instantiate(selectSound);
    }
}
