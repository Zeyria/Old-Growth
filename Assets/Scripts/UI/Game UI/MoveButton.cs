using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveButton : MonoBehaviour
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
		if (!unitcon.moving)
        {
			image.color = Color.white;

			if (transform.GetChild(0).GetComponent<UIIcon>().acting == true)
			{
				transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
				transform.GetChild(0).GetComponent<UIIcon>().acting = false;
				transform.GetChild(4).gameObject.SetActive(false);
			}
		}
		else
		{
			transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(0).GetComponent<UIIcon>().acting = true;
			transform.GetChild(4).gameObject.SetActive(true);
			image.color = Color.yellow;
		}
		if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent <= 0)
        {
			image.color = new Color32(255, 255, 255, 50);
		}

	}

    void TaskOnClick()
	{
		if (transform.GetChild(0).GetComponent<UIIcon>().acting == false)
		{
			if (unitcon.unitToMove.GetComponent<UnitStats>().actionPointCurrent >= 1)
			{
				unitcon.moving = true;
				image.color = Color.yellow;
				transform.GetChild(0).GetComponent<UIIcon>().acting = true;
			}
		}
		else
		{
			unitcon.ClearActions();
		}
	}
}
