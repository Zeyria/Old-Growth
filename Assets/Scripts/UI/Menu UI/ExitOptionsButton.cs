using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitOptionsButton : MonoBehaviour
{
	public Button yourButton;
	public GameObject Main;
	public GameObject Options;
	public bool ifInGame = false;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	public void TaskOnClick()
	{
        if (!ifInGame)
        {
			Main.SetActive(true);
		}
		Options.SetActive(false);
		PlayerPrefs.SetFloat("musicVolume", Config.musicVolume);
		PlayerPrefs.SetFloat("SFXVolume", Config.SFXVolume);
		PlayerPrefs.Save();
		Time.timeScale = 1;
	}
}
