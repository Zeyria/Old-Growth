using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmbarkButton : MonoBehaviour
{
	public Button yourButton;
	public GameObject embarkMenu;
	public GameObject townMenu;
	void Start()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
	{
		embarkMenu.SetActive(!embarkMenu.activeInHierarchy);
		townMenu.SetActive(!townMenu.activeInHierarchy);
	}
}
