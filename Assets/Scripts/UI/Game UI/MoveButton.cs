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
        if (!unitcon.moving)
        {
			image.color = new Color32(74, 60, 74, 255);
			yourButton.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(230, 234, 238, 255);
		}
		if (transform.parent.parent.GetComponent<UnitStats>().actionPointCurrent <= 0)
        {
			image.color = new Color32(0, 0, 0, 0);
			yourButton.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(200, 204, 208, 255);
		}

	}

    void TaskOnClick()
	{
		if(transform.parent.parent.GetComponent<UnitStats>().actionPointCurrent > 0)
        {
			unitcon.moving = true;
			image.color = new Color32(80, 54, 80, 255);
		}
	}
}
