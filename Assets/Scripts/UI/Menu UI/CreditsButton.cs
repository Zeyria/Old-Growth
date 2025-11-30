using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
	public Button yourButton;
	public GameObject creds;
	void Start()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
    {
		creds.SetActive(!creds.activeInHierarchy);
    }
}
