using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Action1Button : MonoBehaviour
{
	public Button yourButton;
	private UnitControler unitcon;
	public Image image;

	void Start()
	{
		unitcon = GameObject.Find("Controller").GetComponent<UnitControler>();
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
	private void Update()
	{
		if (unitcon.unitToMove == null) { return; }
		if (!unitcon.action1)
		{
			image.color = Color.white;

			if (transform.GetChild(1).GetComponent<UIIcon>().acting == true)
			{
				transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
				transform.GetChild(1).GetComponent<UIIcon>().acting = false;
				transform.GetChild(4).gameObject.SetActive(false);
			}
		}
        else
        {
			transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(1).GetComponent<UIIcon>().acting = true;
			transform.GetChild(4).gameObject.SetActive(true);
			image.color = Color.yellow;
		}
		if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent < unitcon.unitToMove.GetComponent<UnitStats>().action1.AP)
		{
			image.color = new Color32(255, 255, 255, 50);
		}
	}

	void TaskOnClick()
	{
		if(unitcon.moving || unitcon.interacting || unitcon.action2) { unitcon.ClearActions();}
		if (transform.GetChild(1).GetComponent<UIIcon>().acting == false)
		{
			if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent >= unitcon.unitToMove.GetComponent<UnitStats>().action1.AP)
			{
				unitcon.action1 = true;
				image.color = Color.yellow;
				transform.GetChild(1).GetComponent<UIIcon>().acting = true;
			}
		}
        else
        {
			unitcon.ClearActions();
        }
	}
}
