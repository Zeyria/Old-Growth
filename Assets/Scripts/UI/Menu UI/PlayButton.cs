using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
	public List<Button> yourButton;
	public string SceneName;
	public UIColorFade fade;
	public float time;

	void Start()
	{
		foreach(Button button in yourButton)
        {
			button.onClick.AddListener(TaskOnClick);
		}
	}

	void TaskOnClick()
	{
		StartCoroutine(Fade());
	}
	IEnumerator Fade()
    {
		yield return new WaitForSeconds(time);
		fade.Trigger();
		yield return new WaitForSeconds(fade.timeSec + fade.startFadeTime);
		StartCoroutine(LoadYourAsyncScene());
    }
    IEnumerator LoadYourAsyncScene()
    {
		foreach (Button button in yourButton)
		{
			button.onClick.AddListener(TaskOnClick);
		}
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
