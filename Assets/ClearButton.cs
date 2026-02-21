using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearButton : MonoBehaviour
{
	public string SceneName;
	public Button yourButton;
	public InteractionDescription interactionDescription;
	void Awake()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
	{
		StartCoroutine(Click());
		//interactionBase.SetActive(false);
	}
	IEnumerator Click()
	{
		interactionDescription.NewText("You heave aside the logs. You and yours can now continue onward.");
		yourButton.interactable = false;
		yield return new WaitForSeconds(4.5f);
		ArenaSettings.activeMapSettings = null;
		ArenaSettings.hasActiveMap = false;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
