using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitToMainMenu : MonoBehaviour
{
	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		StartCoroutine(LoadYourAsyncScene());
		ArenaSettings.hasActiveMap = false;
		ArenaSettings.activeMapSettings = null;
	}
	IEnumerator LoadYourAsyncScene()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
