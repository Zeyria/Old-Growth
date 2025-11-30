using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class ExtinguishFire : MonoBehaviour
{
    public Button yourButton;
	public Button otherButton;
	private UnitControler unitCon;
	public InteractionDescription interactionDescription;
	private GameObject UIHolder;
	public Tile fireOut;
	void Awake()
	{
		yourButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);
		unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
		UIHolder = GameObject.Find("GameUI").transform.GetChild(0).gameObject;
		UIHolder.SetActive(false);
	}
	void TaskOnClick()
	{
		if(Random.Range(0f, 1f) > .33f)
        {
			foreach(GameObject unit in unitCon.allyUnits)
            {
				if(unit != null)
                {
					unit.GetComponent<UnitStats>().sightRange += 1;
				}
            }
			interactionDescription.NewText("As the fire goes out your eyes adjust and you can see further into the darkness.");
        }
        else
        {
			foreach (GameObject unit in unitCon.allyUnits)
			{
				if(unit != null)
                {
					unit.GetComponent<UnitStats>().sightRange -= 1;
				}
			}
			interactionDescription.NewText("The light of the fire goes out and darkness surrounds you. You find it harder to see.");
		}
		unitCon.extraTiles.SetTile(new Vector3Int((int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).x, (int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).y), fireOut);
		unitCon.pathfinding.grid[new Vector3Int((int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).x, (int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).y).x,
			new Vector3Int((int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).x, (int)ComFunc.WorldToTileSpace(transform.parent.GetComponent<interactionData>().pos).y).y].isWalkable = true;
		Destroy(transform.parent.GetComponent<interactionData>().colliderObj.transform.GetChild(0).gameObject);
		yourButton.interactable = false;
		otherButton.interactable = false;
		StartCoroutine(waitThenDestroy());
		unitCon.FogUpdate();
	}
	IEnumerator waitThenDestroy()
    {
		yield return new WaitForSeconds(4.5f);
		UIHolder.SetActive(true);
		Destroy(this.transform.parent.gameObject);
    }
}
