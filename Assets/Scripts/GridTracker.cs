using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTracker : MonoBehaviour
{
    public GameObject menu;
    public GameObject grid;

    private void Update()
    {
        if(grid == null)
        {
            grid = GameObject.Find("ArmyGrid");
        }
        if(grid == null)
        {
            return;
        }
        if (menu.activeInHierarchy)
        {
            grid.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            grid.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
