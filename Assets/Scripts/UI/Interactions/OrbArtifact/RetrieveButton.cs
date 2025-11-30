using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetrieveButton : MonoBehaviour
{
	public string SceneName;
	public Button yourButton;
	public Button otherButton;
	public bool hasOtherButton = true;
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
		interactionDescription.NewText("You brush aside the vines and grab the artifact. It's time to return home.");
		yourButton.interactable = false;
        if (hasOtherButton)
        {
			otherButton.interactable = false;
		}
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
