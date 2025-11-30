using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveInteraction : MonoBehaviour
{
    public GameObject interactionBase;
	public Button yourButton;
	private GameObject UIHolder;
	void Awake()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		UIHolder = GameObject.Find("GameUI").transform.GetChild(0).gameObject;
		UIHolder.SetActive(false);
	}
	void TaskOnClick()
	{
		if(interactionBase.name == "OrbArtifactInteraction")
        {
			if(GameObject.Find("Controller").GetComponent<UnitControler>().gameWinColliderHolder != null)
            {
				GameObject.Find("Controller").GetComponent<UnitControler>().gameWinColliderHolder.SetActive(true);
			}
        }
		UIHolder.SetActive(true);
		Destroy(interactionBase);
	}
}
