using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmbarkButton : MonoBehaviour
{
	public Button yourButton;
	public GameObject embarkMenu;
	public GameObject townMenu;
	public bool rosterChange;
	public bool takeRoster;
	public GameObject rosterHolder;
	public GameObject tempRoster;
	void Start()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
	{
		if (rosterChange)
		{
			if (takeRoster)
			{
				int c = rosterHolder.transform.childCount;
				tempRoster.GetComponent<FillSelect>().availableUnits.Clear();
				for (int i = 0; i < c; i++)
				{
					tempRoster.GetComponent<FillSelect>().availableUnits.Add(rosterHolder.GetComponent<FillSelect>().availableUnits[i]);
					tempRoster.GetComponent<FillSelect>().uiButtons.Add(rosterHolder.transform.GetChild(0).gameObject);
					rosterHolder.transform.GetChild(0).transform.SetParent(tempRoster.transform);
				}
			}
			else
			{
				int c = tempRoster.transform.childCount;
				rosterHolder.GetComponent<FillSelect>().availableUnits.Clear();
				for (int i = 0; i < c; i++)
				{
					rosterHolder.GetComponent<FillSelect>().availableUnits.Add(tempRoster.GetComponent<FillSelect>().availableUnits[i]);
					rosterHolder.GetComponent<FillSelect>().uiButtons.Add(tempRoster.transform.GetChild(0).gameObject);
					tempRoster.transform.GetChild(0).transform.SetParent(rosterHolder.transform);
				}
			}
		}

		embarkMenu.SetActive(!embarkMenu.activeInHierarchy);
		townMenu.SetActive(!townMenu.activeInHierarchy);

	}
}
