using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UITownHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> objs;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(GameObject obj in objs)
        {
            obj.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
    }
}
