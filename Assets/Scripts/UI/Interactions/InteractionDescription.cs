using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionDescription : MonoBehaviour
{
    public TMP_Text text;
    public string startingText;
    private bool crawling;
    public bool usesSound = false;
    public GameObject textSound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textCrawl(startingText));
    }
    public void NewText(string theText)
    {
        StartCoroutine(textCrawl(theText));
    }
    IEnumerator textCrawl(string wholeText)
    {
        string workingText = wholeText;
        text.text = "";
        crawling = !crawling;
        if (!crawling)
        {
            yield return new WaitForSeconds(1f);
        }
        while (workingText.Length > 0)
        {
            if (!crawling)
            {
                text.text = wholeText;
                crawling = false;
                break;
            }

            string subString = workingText.Substring(0, 1);
            workingText = workingText.Remove(0, 1);
            text.text += subString;
            if (usesSound)
            {
                Instantiate(textSound);
            }
            yield return new WaitForSeconds(.04f);
            if(subString == ".")
            {
                yield return new WaitForSeconds(.2f);
            }
        }
        crawling = false;
    }
}
