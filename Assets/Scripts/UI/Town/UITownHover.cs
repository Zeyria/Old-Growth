using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UITownHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public List<GameObject> objs;
    public List<TMP_Text> texts;
    public List<Image> images;
    public float alphaSpeed;
    public Color offColor;
    private bool increaseAlpha;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
        increaseAlpha = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(GameObject obj in objs)
        {
            obj.SetActive(true);
        }
        increaseAlpha = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
        increaseAlpha = false;
    }
    void Update()
    {
        if (increaseAlpha)
        {
            texts[0].color = new Color(1, 1, 1, texts[0].color.a + (alphaSpeed * Time.deltaTime));
            texts[1].color = new Color(offColor.r, offColor.g, offColor.b, texts[1].color.a + (alphaSpeed * Time.deltaTime));
            foreach (Image image in images)
            {
                image.color = new Color(1, 1, 1, image.color.a + (alphaSpeed * Time.deltaTime));
            }
        }
        else
        {
            texts[0].color = new Color(1, 1, 1, 0);
            texts[1].color = new Color(offColor.r, offColor.g, offColor.b, 0);
            foreach (Image image in images)
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
