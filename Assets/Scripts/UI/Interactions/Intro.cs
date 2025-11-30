using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public InteractionDescription interactionDescription;
    int currentText = 0;
    public string SceneName;
    public UIColorFade fade;
    public float time;
    public GameObject art1;
    public GameObject art2;
    public GameObject art3;
    public GameObject art4;
    private void Start()
    {
        StartCoroutine(WaitTimer());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            AdvanceText();
        }
    }
    void AdvanceText()
    {
        if (currentText == 0)
        {
            interactionDescription.NewText("Still fortunes have been made by delving into the woods nearby.");
            art1.SetActive(false);
            art2.SetActive(true);
            currentText++;
        }
        else if (currentText == 1)
        {
            interactionDescription.NewText("So down on your luck you set off with a few of your most loyal knights.");
            art2.SetActive(false);
            art3.SetActive(true);
            currentText++;
        }
        else if (currentText == 2)
        {
            interactionDescription.NewText("However before even arriving the locals decided to test your mettle.");
            art3.SetActive(false);
            art4.SetActive(true);
            currentText++;
        }
        else if (currentText == 3)
        {
            StartCoroutine(Fade());
        }
        StartCoroutine(WaitTimer());
    }
    IEnumerator WaitTimer()
    {
        int currentCached = currentText;
        yield return new WaitForSeconds(7f);
        if(currentText == currentCached)
        {
            AdvanceText();
        }
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
