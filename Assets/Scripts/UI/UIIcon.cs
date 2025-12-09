using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textBox;
    public GameObject selected;
    public GameUIManager gameUIManager;
    public bool acting;
    public void OnPointerEnter(PointerEventData eventData)
    {
        textBox.SetActive(true);
        selected.SetActive(true);
        gameUIManager.activeIcon = transform.GetSiblingIndex();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!acting)
        {
            textBox.SetActive(false);
            selected.SetActive(false);
            gameUIManager.activeIcon = -1;
        }
    }
    private void Update()
    {
        if (acting)
        {
            textBox.SetActive(true);
            selected.SetActive(true);
            gameUIManager.activeIcon = transform.GetSiblingIndex();
        }
    }
}
