using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurn : MonoBehaviour
{
	public Button yourButton;
	public GameObject controler;
	public GameObject hourglass;
	public Sprite hourglassSprite;

	void Start()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}
    private void Update()
    {
        if(controler.GetComponent<UnitControler>().playerTurn == true && hourglass.GetComponent<Image>().sprite == hourglassSprite)
        {
			yourButton.gameObject.GetComponent<Image>().color = Color.white;
			hourglass.GetComponent<Animator>().enabled = false;
			//hourglass.GetComponent<Image>().sprite = hourglassSprite;

			//yourButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(230, 234, 238, 255);
		}
    }
    void TaskOnClick()
	{
        if (controler.GetComponent<UnitControler>().playerTurn)
        {
			hourglass.GetComponent<Animator>().enabled = true;
			controler.GetComponent<UnitControler>().ChangeTurn();
			yourButton.gameObject.GetComponent<Image>().color = new Color32(135, 165, 150, 255);
			//yourButton.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(200, 204, 208, 255);
		}
	}
}
