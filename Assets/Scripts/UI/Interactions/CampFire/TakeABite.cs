using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TakeABite : MonoBehaviour
{
	public Button yourButton;
	public Button otherButton;
	private UnitControler unitCon;
	public InteractionDescription interactionDescription;
	private GameObject UIHolder;
	void Awake()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
		unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
		UIHolder = GameObject.Find("GameUI").transform.GetChild(0).gameObject;
		UIHolder.SetActive(false);
	}
	void TaskOnClick()
	{
		if (Random.Range(0f, 1f) > .33f)
		{
			foreach (GameObject unit in unitCon.allyUnits)
			{
				if(unit != null)
                {
					unit.GetComponent<UnitStats>().hpCurrent += 1;
				}
			}
			interactionDescription.NewText("As you take a bite the food invigorates you, a momentary rest granting you strength.");
		}
		else
		{
			foreach (GameObject unit in unitCon.allyUnits)
			{
				if(unit != null)
                {
					if (unit.GetComponent<UnitStats>().hpCurrent >= 2)
					{
						unit.GetComponent<UnitStats>().hpCurrent -= 1;
					}
				}
			}
			interactionDescription.NewText("The food seems poisoned, you feel unwell.");
		}
		yourButton.interactable = false;
		otherButton.interactable = false;
		StartCoroutine(waitThenDestroy());
		unitCon.FogUpdate();
	}
	IEnumerator waitThenDestroy()
	{
		yield return new WaitForSeconds(4.5f);
		UIHolder.SetActive(true);
		Destroy(this.transform.parent.gameObject);
	}
}
