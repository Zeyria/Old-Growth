using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenURL : MonoBehaviour, IPointerClickHandler
{
    public string URL;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(URL != "")
        {
            Application.OpenURL(URL);
        }
    }
}
