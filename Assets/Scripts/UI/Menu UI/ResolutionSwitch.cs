using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSwitch : MonoBehaviour
{
	public Button left;
	public Button right;
	public List<Vector2> resList;
	public TMP_Text resText;
	private int resIndex = 2;

	void Start()
	{
		resIndex = Config.ResolutionIndex;
		//SetRes();
		left.GetComponent<Button>().onClick.AddListener(LeftTaskOnClick);
		right.GetComponent<Button>().onClick.AddListener(RightTaskOnClick);
	}

	void LeftTaskOnClick()
	{
		resIndex--;
		if(resIndex < 0)
        {
			resIndex = 0;
        }
		SetRes();
	}
	void RightTaskOnClick()
	{
		resIndex++;
		if(resIndex > resList.Count - 1)
        {
			resIndex = resList.Count - 1;
        }
		SetRes();
	}
	void SetRes()
    {
		Screen.SetResolution((int)resList[resIndex].x, (int)resList[resIndex].y, Config.Fullscreen);
		resText.text = resList[resIndex].x + " x " + resList[resIndex].y;
		Config.ResolutionIndex = resIndex;

	}
}
