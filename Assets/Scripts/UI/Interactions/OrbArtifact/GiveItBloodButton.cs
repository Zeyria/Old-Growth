using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiveItBloodButton : MonoBehaviour
{
	public Button yourButton;
	public Button otherButton;
	public InteractionDescription interactionDescription;
	private GameObject UIHolder;
	public GameObject survivalCon;
	void Awake()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		UIHolder = GameObject.Find("GameUI").transform.GetChild(0).gameObject;
		UIHolder.SetActive(false);
	}
	void TaskOnClick()
	{
		StartCoroutine(Click());
		//interactionBase.SetActive(false);
	}
	IEnumerator Click()
	{
		interactionDescription.NewText("Vines begin to slowly drink the blood you put into the basin. This looks like it may take some time.");
		yourButton.interactable = false;
		otherButton.interactable = false;
		yield return new WaitForSeconds(6f);
		Destroy(this.transform.parent.gameObject);
		UIHolder.SetActive(true);
		Instantiate(survivalCon, GameObject.Find("Game").transform);
	}
}
