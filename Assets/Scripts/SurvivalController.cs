using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SurvivalController : MonoBehaviour
{
    private TMP_Text objectiveText;
    public int roundsLeft;
    public string SceneName;
    public GameObject gameWinInteraction;
    public GameObject wildlifeSpawner;
    private UnitControler unitCon;
    public GameObject spawnSound;
    private bool firstRound;
    private void Start()
    {
        objectiveText = GameObject.Find("ObjectiveText 1").GetComponent<TMP_Text>();
        unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
        roundsLeft += 1;
        firstRound = true;
        RoundTick();
    }
    public void RoundTick()
    {
        roundsLeft -= 1;
        objectiveText.text = "Objective: Survive <color=red>"+roundsLeft+"</color> rounds.";
        if(roundsLeft == 1)
        {
            objectiveText.text = "Objective: Survive <color=red>" + roundsLeft + "</color> round.";
        }
        if(roundsLeft <= 0)
        {
            objectiveText.text = "Objective: Return to the artifact.";
            if (unitCon.gameWinColliderHolder != null)
            {
                unitCon.gameWinColliderHolder.SetActive(true);
                unitCon.gameWinColliderHolder.GetComponent<Interactable>().interactableGameObject = gameWinInteraction;
            }
        }
        if (!firstRound)
        {
            StartCoroutine(Spawning());
        }
        firstRound = false;
    }
    IEnumerator Spawning()
    {
        int attempts = 0;
        int spawns = 0;
        while (attempts < 1000)
        {
            int x = Random.Range(0, unitCon.pathfinding.grid.GetLength(0));
            int y = Random.Range(0, (int)unitCon.pathfinding.grid.GetLength(1));
            //Debug.Log(x + "   " + y);
            if (unitCon.pathfinding.grid[x, y].isWalkable && spawns < 4 && unitCon.floorTiles.GetColor(new Vector3Int(x, y)) == Color.white)
            {
                spawns++;
                //Debug.Log("Spawned");
                GameObject a = Instantiate(wildlifeSpawner, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) + new Vector3(0, .25f), this.transform.rotation, unitCon.transform.parent.GetChild(1).transform);
                a.GetComponent<Spawner>().addToEnemyUnits = true;
                unitCon.pathfinding.grid[x, y].isWalkable = false;
                unitCon.AIsDone++;
                Instantiate(spawnSound);
                yield return new WaitForSeconds(.25f);
            }
            attempts++;
        }
        unitCon.FogUpdate();
    }
}
