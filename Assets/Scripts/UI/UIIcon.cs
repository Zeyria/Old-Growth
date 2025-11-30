using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textBox;
    public GameUIManager gameUIManager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        textBox.SetActive(true);
        gameUIManager.activeIcon = transform.GetSiblingIndex();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        textBox.SetActive(false);
        gameUIManager.activeIcon = -1;
    }
}
