using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorFade : MonoBehaviour
{
    public bool waitForTrigger = false;
    public bool FadeInTrueOutFalse = false;
    public float startFadeTime;
    public float timeSec;
    public Image image;
    void Start()
    {
        Time.timeScale = 1;
        if (!waitForTrigger)
        {
            StartCoroutine(StartFadeAfter());
        }
    }
    public void Trigger()
    {
        StartCoroutine(StartFadeAfter());
    }
    IEnumerator StartFadeAfter()
    {
        yield return new WaitForSeconds(startFadeTime);
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    {
        float elapsedTime = 0;
        while(elapsedTime < timeSec)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            elapsedTime += Time.deltaTime;
            if (FadeInTrueOutFalse)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (elapsedTime / timeSec));
            }
            else
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - (elapsedTime / timeSec));
            }
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
