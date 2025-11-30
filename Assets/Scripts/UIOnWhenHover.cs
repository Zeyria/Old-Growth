using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIOnWhenHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject selectedGameObj;
    public List<GameObject> gameObjects;
    bool doOnce;
    bool inTown;
    void Start()
    {
        inTown = true;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (!inTown)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (!inTown)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Town")
        {
            inTown = true;
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            inTown = false;
        }
        if (!inTown)
        {
            if (selectedGameObj.activeInHierarchy)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.SetActive(true);
                }
                doOnce = true;
            }
            else if (doOnce)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.SetActive(false);
                }
                doOnce = false;
            }
        }
    }
}
