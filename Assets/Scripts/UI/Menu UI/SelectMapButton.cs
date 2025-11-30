using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapButton : MonoBehaviour
{
	public Button yourButton;
	public GameObject map;
	public GameObject playButton;
	public GameObject unitHolder;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
	{
		map.SetActive(!map.gameObject.activeInHierarchy);
	}
    private void Update()
    {
        if (!ArenaSettings.hasActiveMap)
        {
			playButton.GetComponent<Button>().enabled = false;
			playButton.transform.GetChild(2).gameObject.SetActive(true);
			playButton.transform.GetChild(0).gameObject.SetActive(false);
		}
        else
        {
			playButton.GetComponent<Button>().enabled = true;
			playButton.transform.GetChild(2).gameObject.SetActive(false);
			playButton.transform.GetChild(0).gameObject.SetActive(true);
		}
		if (unitHolder == null)
		{
			unitHolder = GameObject.Find("ArmyGrid").transform.GetChild(0).gameObject;
		}
		if(unitHolder != null && playButton.transform.GetChild(2).gameObject.activeInHierarchy == false)
        {
			if(unitHolder.transform.childCount < 3)
            {
				playButton.GetComponent<Button>().enabled = false;
				playButton.transform.GetChild(3).gameObject.SetActive(true);
				playButton.transform.GetChild(0).gameObject.SetActive(false);
			}
			else if (unitHolder.transform.childCount > 7)
            {
				playButton.GetComponent<Button>().enabled = false;
				playButton.transform.GetChild(4).gameObject.SetActive(true);
				playButton.transform.GetChild(0).gameObject.SetActive(false);
			}
            else
            {
				playButton.GetComponent<Button>().enabled = true;
				playButton.transform.GetChild(3).gameObject.SetActive(false);
				playButton.transform.GetChild(4).gameObject.SetActive(false);
				playButton.transform.GetChild(0).gameObject.SetActive(true);
			}
        }
	}
}
