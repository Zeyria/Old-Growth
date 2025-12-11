using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractButton : MonoBehaviour
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
		if (!unitcon.interacting)
		{
			image.color = Color.white;

			if (transform.GetChild(3).GetComponent<UIIcon>().acting == true)
			{
				transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
				transform.GetChild(3).GetComponent<UIIcon>().acting = false;
				transform.GetChild(4).gameObject.SetActive(false);
			}
		}
		else
		{
			transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(3).GetComponent<UIIcon>().acting = true;
			transform.GetChild(4).gameObject.SetActive(true);
			image.color = Color.yellow;
		}
		if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent < 2)
		{
			image.color = new Color32(255, 255, 255, 50);
		}
	}

	void TaskOnClick()
	{
		if (unitcon.moving || unitcon.action1 || unitcon.action2) { unitcon.ClearActions(); }
		if (transform.GetChild(3).GetComponent<UIIcon>().acting == false)
		{
			if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent >= 2)
			{
				unitcon.interacting = true;
				image.color = Color.yellow;
				transform.GetChild(3).GetComponent<UIIcon>().acting = true;
			}
		}
		else
		{
			unitcon.ClearActions();
		}
	}
}
