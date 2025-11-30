using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameESCMenu : MonoBehaviour
{
    public GameObject escMenu;
    public ExitOptionsButton exitbutton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenu.SetActive(!escMenu.activeInHierarchy);
            if (escMenu.activeInHierarchy)
            {
                Time.timeScale = 0;
            }
            else
            {
                exitbutton.TaskOnClick();
            }
        }
    }
}
