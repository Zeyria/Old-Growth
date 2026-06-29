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
		unitHolder = GameObject.Find("ArmyGrid").transform.GetChild(0).gameObject;
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
			playButton.transform.GetChild(1).gameObject.SetActive(true);
			playButton.transform.GetChild(2).gameObject.SetActive(false);
			playButton.transform.GetChild(3).gameObject.SetActive(false);
			playButton.transform.GetChild(0).gameObject.SetActive(false);
		}
        else
        {
			playButton.GetComponent<Button>().enabled = true;
			playButton.transform.GetChild(1).gameObject.SetActive(false);
			playButton.transform.GetChild(0).gameObject.SetActive(true);
		}
		if (unitHolder == null)
		{
			unitHolder = GameObject.Find("ArmyGrid").transform.GetChild(0).gameObject;
		}
		if(unitHolder != null && playButton.transform.GetChild(1).gameObject.activeInHierarchy == false)
        {
			int c = 0;
            for (int i = 0; i < unitHolder.transform.childCount; i++)
            {
                if (unitHolder.transform.GetChild(i).gameObject.activeInHierarchy)
                {
					c++;
                }
            }
			if(c < 3)
            {
				playButton.GetComponent<Button>().enabled = false;
				playButton.transform.GetChild(1).gameObject.SetActive(false);
				playButton.transform.GetChild(2).gameObject.SetActive(true);
				playButton.transform.GetChild(3).gameObject.SetActive(false);
				playButton.transform.GetChild(0).gameObject.SetActive(false);
			}
			else if (c > 7)
            {
				playButton.GetComponent<Button>().enabled = false;
				playButton.transform.GetChild(1).gameObject.SetActive(false);
				playButton.transform.GetChild(2).gameObject.SetActive(false);
				playButton.transform.GetChild(3).gameObject.SetActive(true);
				playButton.transform.GetChild(0).gameObject.SetActive(false);
			}
            else
            {
				playButton.GetComponent<Button>().enabled = true;
				playButton.transform.GetChild(2).gameObject.SetActive(false);
				playButton.transform.GetChild(3).gameObject.SetActive(false);
				playButton.transform.GetChild(0).gameObject.SetActive(true);
			}
        }
	}
}
