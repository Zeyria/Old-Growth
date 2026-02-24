using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseDropWindow : MonoBehaviour, IPointerClickHandler
{
    public GameObject dropMarker;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Time.timeScale = 1;
        if(dropMarker != null)
        {
            Destroy(dropMarker);
        }
        Destroy(transform.root.gameObject);
    }
}
