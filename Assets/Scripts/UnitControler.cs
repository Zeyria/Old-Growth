using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class UnitControler : MonoBehaviour
{
    public ScreenShake cameraSS;
    public MapMaker maker;
    public Tilemap floorTiles;
    public Tilemap extraTiles;
    public GameObject selectCursor;
    public Texture2D cursorTex;
    public GameObject cameraM;
    public TileBase halfTile;
    public Color fog;
    public Color fogSeen;
    public GameObject selectT;
    public GameObject selectR;
    public GameObject selectBL;
    public GameObject selectBR;
    public GameObject selectL;
    public GameObject stepSound;
    public GameObject corpse;
    public GameObject gameOver;
    public GameObject gameOverFadeIn;
    //public GameObject gameWin;
    public RawImage portraitImage;
    public GameObject UIHolder;
    public bool debugSeePathfinding;
    public GameObject damageNumber;
    public GameObject gameWinColliderHolder;
    public GameObject selectSound;
    public bool artifactPulse = true;

    [HideInInspector]
    public GameObject unitToMove;
    [HideInInspector]
    public AStarPathfinding pathfinding;
    [HideInInspector]
    public bool moving;
    [HideInInspector]
    public bool action1;
    [HideInInspector]
    public bool action2;
    [HideInInspector]
    public bool interacting;
    [HideInInspector]
    public bool playerTurn;
    [HideInInspector]
    public bool overUI;
    [HideInInspector]
    public List<GameObject> allyUnits;
    [HideInInspector]
    public GameObject allyUnitsHolder;
    [HideInInspector]
    public List<GameObject> enemyUnits;

    private int highlight;
    private AStarPathfinding temp;
    private Vector2 pos;
    [HideInInspector]
    public int AIsDone;

    private Vector3 origin;
    private Vector3 difference;
    private bool drag;

    private void Awake()
    {
        enemyUnits = new List<GameObject>();
        action1 = false;
        moving = false;
        playerTurn = true;
        highlight = 1;
        Cursor.SetCursor(cursorTex, new Vector2(1f, 1f), CursorMode.ForceSoftware);
        unitToMove = null;
        pathfinding = new AStarPathfinding((int)maker.MapSize * 7 + 10, (int)maker.MapSize * 7 + 10);
        if (GameObject.Find("ArmyGrid") != null)
        {
            allyUnitsHolder = GameObject.Find("ArmyGrid").transform.GetChild(0).gameObject;
        }
    }
    private void Start()
    {
        StartCoroutine(AfterStart());

        //Temp is needed to not pollute the real pathfinding.
         temp = new AStarPathfinding((int)maker.MapSize * 7 + 10, (int)maker.MapSize * 7 + 10);
        for (int x = 0; x < temp.grid.GetLength(0); x++)
        {
            for (int y = 0; y < temp.grid.GetLength(1); y++)
            {
                temp.grid[x, y].isWalkable = true;
            }
        }
    }
    IEnumerator AfterStart()
    {
        yield return new WaitForSeconds(.05f);
        if (allyUnitsHolder != null)
        {
            for (int c = 0; c < allyUnitsHolder.transform.childCount; c++)
            {
                allyUnitsHolder.transform.GetChild(c).SetParent(floorTiles.transform.parent);
            }

        }
        for (int c = 3; c <= floorTiles.transform.parent.childCount; c++)
        {
            GameObject tempUnit = floorTiles.transform.parent.GetChild(c - 1).gameObject;
            pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(tempUnit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(tempUnit.transform.position).y)].isWalkable = false;
            if(tempUnit.GetComponent<UnitStats>() != null)
            {
                tempUnit.GetComponent<UnitStats>().hpCurrent = tempUnit.GetComponent<UnitStats>().hpMax;
                if (!tempUnit.GetComponent<UnitStats>().isEnemy)
                {
                    allyUnits.Add(tempUnit);
                }
                else if (tempUnit.GetComponent<UnitStats>().isEnemy)
                {
                    enemyUnits.Add(tempUnit);
                }
            }
        }
        FogUpdate();
        maker.startSquare.transform.parent.gameObject.SetActive(true);
    }
    void Update()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = ComFunc.WorldToGridSpace(pos.x, pos.y);
        Tile select = (Tile)floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(pos.y * 7 - .5f), Mathf.RoundToInt(pos.x * 7 - .5f)));
        if (select != null && floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(pos.y * 7 - .5f), Mathf.RoundToInt(pos.x * 7 - .5f))) != fog)
        {
            pos = new Vector2(Mathf.RoundToInt(pos.y * 7 - .5f), Mathf.RoundToInt(pos.x * 7 - .5f));
            pos = pos / 7;
            pos = new Vector3((pos.y * -3.5f) + (pos.x * 3.5f), (pos.x * 1.75f + (pos.y * 1.75f)) + .25f, 0);
            if (Time.timeScale == 1)
            {
                selectCursor.transform.position = pos;
                selectCursor.SetActive(true);
            }
            else
            {
                selectCursor.SetActive(false);
            }
        }
        else
        {
            selectCursor.SetActive(false);
        }
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!drag)
        {
            cameraM.transform.position = new Vector3(cameraM.transform.position.x + (20 * moveHorizontal * Time.deltaTime), cameraM.transform.position.y + (20 * moveVertical * Time.deltaTime), -10);
        }
        if(Input.mouseScrollDelta.y != 0)
        {
            cameraM.GetComponent<Camera>().orthographicSize -= Input.mouseScrollDelta.y;
            cameraM.GetComponent<Camera>().orthographicSize = Mathf.Clamp(cameraM.GetComponent<Camera>().orthographicSize, 3, 10);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 averageVec = new Vector2();
            foreach (GameObject unit in allyUnits)
            {
                averageVec += new Vector2(unit.transform.position.x, unit.transform.position.y);
            }
            averageVec = new Vector2(averageVec.x / allyUnits.Count, averageVec.y / allyUnits.Count);
            cameraM.transform.position = averageVec;
        }

        if (unitToMove != null && !moving && !action1 && !action2)
        {
            selectCursor.SetActive(false);
        }
        if(unitToMove != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !unitToMove.GetComponent<UnitStats>().isEnemy && unitToMove.GetComponent<UnitStats>().actionPointCurrent >= 1) { moving = true; Instantiate(selectSound); }
            if (Input.GetKeyDown(KeyCode.Alpha2) && !unitToMove.GetComponent<UnitStats>().isEnemy && unitToMove.GetComponent<UnitStats>().actionPointCurrent >= unitToMove.GetComponent<UnitStats>().action1.AP) { action1 = true; Instantiate(selectSound); }
            if (Input.GetKeyDown(KeyCode.Alpha3) && !unitToMove.GetComponent<UnitStats>().isEnemy && unitToMove.GetComponent<UnitStats>().actionPointCurrent >= unitToMove.GetComponent<UnitStats>().action2.AP) { action2 = true; Instantiate(selectSound); }
            if (Input.GetKeyDown(KeyCode.Alpha4) && !unitToMove.GetComponent<UnitStats>().isEnemy && unitToMove.GetComponent<UnitStats>().actionPointCurrent >= 3) { interacting = true; Instantiate(selectSound); }
        }
        //Highlights
        if ((action1 && highlight != 0) || (action2 && highlight != 0) || (interacting && highlight != 0) || (moving && highlight != 0))
        {
            if (unitToMove != null)
            {
                unitToMove.transform.GetChild(0).gameObject.SetActive(false);
                if (action1) { Highlights(unitToMove.GetComponent<UnitStats>().action1.range, unitToMove, false, unitToMove.GetComponent<UnitStats>().action1.minRange); }
                if (action2) { Highlights(unitToMove.GetComponent<UnitStats>().action2.range, unitToMove, false, unitToMove.GetComponent<UnitStats>().action2.minRange); }
                if (interacting) 
                { 
                    Highlights(3, unitToMove, false);
                    for (int i = 0; i < transform.parent.GetChild(1).childCount; i++)
                    {
                        if(transform.parent.GetChild(1).GetChild(i).GetComponent<Interactable>() != null)
                        {
                            Vector2 pos = transform.parent.GetChild(1).GetChild(i).transform.position;
                            if (floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(pos).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(pos).y), 0)) != fog)
                            {
                                transform.parent.GetChild(1).GetChild(i).GetChild(0).gameObject.SetActive(true);
                            }
                        }
                    }
                }
                if (moving)
                {
                    Highlights(unitToMove.GetComponent<UnitStats>().speed, unitToMove, true);
                    pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y)].isWalkable = false;
                }
                highlight = 0;
            }
        }
        //Clear Highlights
        if ((!action1 && highlight == 1) || (!moving && highlight == 1) || (!action2 && highlight == 1))
        {
            ClearHighlights();
            highlight = 2;
        }
        if (Input.GetMouseButtonDown(0) && playerTurn && (!overUI || moving || interacting || action1 || action2))
        {
            if (unitToMove == null)
            {
                GameObject gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(selectCursor.transform.position).x, (int)ComFunc.WorldToTileSpace(selectCursor.transform.position).y));
                if (!selectCursor.activeInHierarchy)
                {
                    gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, (int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y));
                }
                if (gameObject != null)
                {
                    unitToMove = gameObject;
                    Instantiate(selectSound);
                    if (!unitToMove.GetComponent<UnitStats>().isCorpse && unitToMove.GetComponent<SpriteRenderer>().enabled)
                    {
                        unitToMove.transform.GetChild(0).gameObject.SetActive(true);
                        pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y)].isWalkable = false;
                    }
                    else
                    {
                        unitToMove = null;
                    }
                }
            }
            else if (moving)
            {
                MoveUnit();
                moving = false;
                highlight = 1;
                Instantiate(selectSound);
            }
            else if (action1)
            {
                Action(1);
                action1 = false;
                highlight = 1;
                Instantiate(selectSound);
            }
            else if (action2)
            {
                Action(2);
                action2 = false;
                highlight = 1;
                Instantiate(selectSound);
            }
            else if (interacting)
            {
                Interact();
                interacting = false;
                highlight = 1;
                Instantiate(selectSound);
            }
            else
            {
                GameObject gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(selectCursor.transform.position).x, (int)ComFunc.WorldToTileSpace(selectCursor.transform.position).y));
                if (!selectCursor.activeInHierarchy)
                {
                    gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, (int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y));
                }
                if(gameObject == null)
                {
                    Instantiate(selectSound);
                    moving = false;
                    action1 = false;
                    action2 = false;
                    interacting = false;
                    highlight = 1;
                    if (!unitToMove.GetComponent<UnitStats>().isCorpse)
                    {
                        unitToMove.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    unitToMove = null;
                }
                else
                {
                    Instantiate(selectSound);
                    moving = false;
                    action1 = false;
                    action2 = false;
                    interacting = false;
                    highlight = 1;
                    if (!unitToMove.GetComponent<UnitStats>().isCorpse)
                    {
                        unitToMove.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    if(GameObject.Find("Unit Name") != null)
                    {
                        GameObject.Find("Unit Name").GetComponent<TMP_InputField>().text = " ";
                    }
                    unitToMove = gameObject;
                    if (!unitToMove.GetComponent<UnitStats>().isCorpse && unitToMove.GetComponent<SpriteRenderer>().enabled)
                    {
                        unitToMove.transform.GetChild(0).gameObject.SetActive(true);
                        pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y)].isWalkable = false;
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && playerTurn)
        {
            if (unitToMove != null)
            {
                Instantiate(selectSound);
                moving = false;
                action1 = false;
                action2 = false;
                interacting = false;
                highlight = 1;
                if (!unitToMove.GetComponent<UnitStats>().isCorpse)
                {
                    unitToMove.transform.GetChild(0).gameObject.SetActive(false);
                }
                unitToMove = null;
            }
        }
        if (AIsDone >= enemyUnits.Count)
        {
            playerTurn = true;
            FogUpdate();
        }
        DebuggingTools();
        if(unitToMove == null)
        {
            overUI = false;
        }
    }
    private void LateUpdate()
    {
        //Middle Mouse drag
        Vector3 mousePos = new Vector3();
        Vector3 dragVelocity = new Vector3();
        if (Mathf.Abs(mousePos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) > .15f || Mathf.Abs(mousePos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y) > .15f)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            if (drag == false)
            {
                drag = true;
                origin = mousePos;
            }
            difference = origin - mousePos;
            Vector3.SmoothDamp(difference, mousePos, ref dragVelocity, .2f, 250f, Time.deltaTime);
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            if(difference.magnitude < .05f)
            {
                return;
            }
            Camera.main.transform.position += new Vector3(difference.x, difference.y, 0) * dragVelocity.magnitude * Time.deltaTime;
        }
    }
    public void ChangeTurn()
    {
        playerTurn = !playerTurn;
        if(unitToMove != null)
        {
            unitToMove.transform.GetChild(0).gameObject.SetActive(false);
            unitToMove = null;
        }
        GameObject map = transform.parent.GetChild(1).gameObject;
        EnemyAI();
        for (int i = 2; i <= map.transform.childCount - 1; i++)
        {
            if(map.transform.GetChild(i).GetComponent<UnitStats>() != null)
            {
                if (!map.transform.GetChild(i).GetComponent<UnitStats>().isEnemy)
                {
                    map.transform.GetChild(i).GetComponent<UnitStats>().actionPointCurrent = map.transform.GetChild(i).GetComponent<UnitStats>().actionPointMax;

                }
                if (map.transform.GetChild(i).TryGetComponent<CorpseLogic>(out CorpseLogic corpseLogic))
                {
                    corpseLogic.RoundTick();
                }
            }
        }
        GameObject game = GameObject.Find("Game");
        for (int  i = 0;  i < game.transform.childCount;  i++)
        {
            if(game.transform.GetChild(i).GetComponent<SurvivalController>() != null)
            {
                game.transform.GetChild(i).GetComponent<SurvivalController>().RoundTick();
            }
        }
        if (artifactPulse)
        {
            this.gameObject.GetComponent<ArtifactPulse>().RoundTick();
        }
    }
    void EnemyAI()
    {
        AIsDone = 0;
        List<GameObject> removes = new List<GameObject>();
        foreach(GameObject unit in enemyUnits)
        {
            if(unit != null && !unit.GetComponent<UnitStats>().isCorpse)
            {
                StartCoroutine(AIHelper(unit));
            }
            else
            {
                if(unit == null)
                {
                    removes.Add(unit);
                }
                AIsDone++;
            }
        }
        foreach(GameObject unit in removes)
        {
            enemyUnits.Remove(unit);
            AIsDone--;
        }
    }
    void MoveUnit()
    {
        if (unitToMove != null && selectCursor.activeInHierarchy)
        {
            unitToMove.transform.GetChild(0).gameObject.SetActive(true);
            AStarNode[,] cashedPathfinding = pathfinding.grid;

            foreach(GameObject unit in allyUnits)
            {
                if(unit != null)
                {
                    pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y)].isWalkable = true;
                }
            }

            List<AStarNode> path = pathfinding.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y),
                Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y));

            foreach (GameObject unit in allyUnits)
            {
                if(unit != null)
                {
                    pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y)].isWalkable = false;
                }
            }

            if (path != null && path.Count <= unitToMove.GetComponent<UnitStats>().speed && cashedPathfinding[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y].isWalkable)
            {
                unitToMove.GetComponent<UnitStats>().actionPointCurrent -= 1;
                StartCoroutine(MoveWaiter(path, unitToMove));
            }
        }
    }
    void Action(int actionNum)
    {
        ActionClass action = null;
        if (actionNum == 1) { action = unitToMove.GetComponent<UnitStats>().action1; }
        if (actionNum == 2) { action = unitToMove.GetComponent<UnitStats>().action2; }
        if (unitToMove != null)
        {
            int range = action.range;
            int minRange = action.minRange;
            float damageMult = action.damageMult;
            GameObject animation = action.animationPrefab;
            GameObject sound = action.soundPrefab;

            if(action.targetingType == 0) //Enemies targeting
            {
                GameObject unitToAttack = null;
                if (unitToAttack == null)
                {
                    GameObject gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(selectCursor.transform.position).x, (int)ComFunc.WorldToTileSpace(selectCursor.transform.position).y));
                    if (!selectCursor.activeInHierarchy)
                    {
                        gameObject = GetUnitAtTile(new Vector3Int((int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, (int)ComFunc.WorldToTileSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y));
                    }
                    if (gameObject != null)
                    {
                        unitToAttack = gameObject;
                        if (unitToAttack != unitToMove && unitToAttack.GetComponent<UnitStats>().isEnemy)
                        {
                            List<AStarNode> path = temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y),
                Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).y));
                            if (path != null)
                            {
                                if (path.Count <= range && path.Count >= minRange)
                                {
                                    int damage = Mathf.RoundToInt(unitToMove.GetComponent<UnitStats>().attack * damageMult) - Random.Range(-1, 2); 
                                    unitToMove.GetComponent<UnitStats>().actionPointCurrent -= action.AP;
                                    cameraSS.start = true;
                                    unitToMove.transform.GetChild(0).gameObject.SetActive(true);
                                    unitToAttack.GetComponent<UnitStats>().hpCurrent -= damage;
                                    if (unitToAttack.GetComponent<UnitStats>().hpCurrent <= 0)
                                    {
                                        if (!unitToAttack.GetComponent<UnitStats>().isCorpse && unitToAttack.GetComponent<UnitStats>().spawnsCorpse)
                                        {
                                            GameObject a = Instantiate(corpse, unitToAttack.transform.position, unitToAttack.transform.rotation, transform.parent.GetChild(1));
                                            enemyUnits.Add(a);
                                        }
                                        else
                                        {
                                            pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).y)].isWalkable = true;
                                        }
                                        Destroy(unitToAttack);
                                    }
                                    else
                                    {
                                        StartCoroutine(CheckEffects(unitToMove, action, unitToAttack));
                                    }
                                    GameObject b = Instantiate(damageNumber, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                                    b.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
                                    Instantiate(animation, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                                    Instantiate(sound, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                                }
                            }
                        }
                    }
                }
            }
            else if(action.targetingType == 1) //Ally targeting
            {

            }
            else if(action.targetingType == 2)//Open tile targeting
            {
                if (unitToMove != null && selectCursor.activeInHierarchy)
                {
                    unitToMove.transform.GetChild(0).gameObject.SetActive(true);
                    List<AStarNode> path = temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y),
                        Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(selectCursor.transform.position).y));
                    if (path != null && path.Count <= action.range)
                    {
                        unitToMove.GetComponent<UnitStats>().actionPointCurrent -= action.AP;
                        StartCoroutine(CheckEffects(unitToMove, action, targetTile: path[path.Count - 1]));
                    }
                }
            }
        }
        action1 = false;
    }
    void Interact()
    {
        GameObject interactable = null;
        if(unitToMove != null)
        {
            int layerMask = (1 << 7);
            Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(ray, ray, 1, layerMask);

            if(hit.collider != null)
            {
                interactable = hit.collider.gameObject;

                List<AStarNode> path = temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToMove.transform.position).y),
                Mathf.RoundToInt(ComFunc.WorldToTileSpace(interactable.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(interactable.transform.position).y));

                if(path != null)
                {
                    //Debug.Log("path:" + path.Count);
                    if (path.Count <= 5)
                    {
                        GameObject a = Instantiate(hit.collider.GetComponent<Interactable>().interactableGameObject, UIHolder.transform.parent);
                        a.GetComponent<interactionData>().pos = hit.collider.transform.position;
                        a.GetComponent<interactionData>().colliderObj = hit.collider.gameObject;
                        if(hit.collider.GetComponent<Interactable>().interactableGameObject.name != "OrbArtifactInteraction")
                        {
                            Destroy(hit.collider.gameObject.GetComponent<Interactable>());
                            Destroy(hit.collider.gameObject.GetComponent<CircleCollider2D>());
                            Destroy(hit.collider.transform.GetChild(0).gameObject);
                        }
                        else
                        {
                            gameWinColliderHolder = hit.collider.gameObject;
                            gameWinColliderHolder.SetActive(false);
                        }
                        unitToMove.GetComponent<UnitStats>().actionPointCurrent -= 3;
                    }
                }
            }
        }
    }
    public void FogUpdate()
    {
        //Getting a blank slate before checking vision
        BoundsInt boundsInt = new BoundsInt(Vector3Int.zero, new Vector3Int((int)maker.MapSize * 7, (int)maker.MapSize * 7, 1));
        Tilemap floor = floorTiles.transform.GetComponent<Tilemap>();
        Tilemap extra = extraTiles.transform.GetComponent<Tilemap>();

        foreach (var tile in boundsInt.allPositionsWithin)
        {
            if (floor.GetTile(tile) != null)
            {
                if (floorTiles.GetColor(tile) == Color.white)
                {
                    floorTiles.SetColor(tile, fogSeen);
                }
            }
            if (extra.GetTile(tile) != null)
            {
                if (extraTiles.GetColor(tile) == Color.white)
                {
                    extraTiles.SetColor(tile, fogSeen);
                }
            }
        }

        foreach (GameObject unit in allyUnits)
        {
            if (unit != null)
            {
                int sight = unit.GetComponent<UnitStats>().sightRange;
                Vector2 unitPos = ComFunc.WorldToTileSpace(unit.transform.position);
                //Vision fall off
                if (unit.GetComponent<UnitStats>().isCorpse)
                {
                    if(floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x), Mathf.RoundToInt(unitPos.y))) != Color.white)
                    {
                        unit.GetComponent<SpriteRenderer>().enabled = false;
                        unit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = false;
                        continue;
                    }
                    else { unit.GetComponent<SpriteRenderer>().enabled = true; unit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = true; }
                }

                for (int x = -sight; x < sight; x++)
                {
                    for (int y = -sight; y < sight; y++)
                    {
                        if(x < -sight - 4 && x > sight + 4) { continue; }
                        if (y < -sight - 4 && y > sight + 4) { continue; }
                        if (Mathf.RoundToInt(unitPos.x) + x < 0 || Mathf.RoundToInt(unitPos.y) + y < 0
                            || x + unitPos.x > (int)maker.MapSize * 7 + 2 || y + unitPos.y > (int)maker.MapSize * 7 + 2) { continue; }
                        if (floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) == fogSeen) { continue; }

                        int distance = pathfinding.CalculateDistance(pathfinding.grid[Mathf.RoundToInt(unitPos.x), Mathf.RoundToInt(unitPos.y)], pathfinding.grid[Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y]);
                        if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != null)
                        {
                            if (distance <= sight * 10)
                            {
                                if (floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != Color.white)
                                {
                                    floorTiles.SetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y), fogSeen);
                                }
                            }
                        }
                        if (extraTiles.GetTile(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != null)
                        {
                            if (distance <= sight * 10)
                            {
                                if (extraTiles.GetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != Color.white)
                                {
                                    extraTiles.SetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y), fogSeen);
                                }
                            }
                        }
                    }
                }
                //Real vision
                for (int x = -sight + 4; x <= sight - 4; x++)
                {
                    for (int y = -sight + 4; y <= sight - 4; y++)
                    {
                        if (Mathf.RoundToInt(unitPos.x) + x < 0 || Mathf.RoundToInt(unitPos.y) + y < 0
                            || x + unitPos.x > (int)maker.MapSize * 7 + 2 || y + unitPos.y > (int)maker.MapSize * 7 + 2) { continue; }

                        int distance = pathfinding.CalculateDistance(pathfinding.grid[Mathf.RoundToInt(unitPos.x), Mathf.RoundToInt(unitPos.y)], pathfinding.grid[Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y]);
                        if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != null)
                        {
                            if (distance <= sight * 10)
                            {
                                floorTiles.SetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y), Color.white);

                                int layerMask = (1 << 6);
                                Vector2 ray = ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y).x, ComFunc.TileToGridSpace(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y).y);
                                RaycastHit2D hit = Physics2D.Raycast(ray, ray, 1, layerMask);
                                if (hit.collider != null)
                                {
                                    hit.transform.GetComponent<SpriteRenderer>().enabled = true;
                                    if (!hit.collider.GetComponent<UnitStats>().isCorpse)
                                    {
                                        hit.transform.GetChild(2).GetComponent<SpriteMask>().enabled = true;
                                    }
                                    else { hit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = true; }

                                }
                            }
                        }
                        if(extraTiles.GetTile(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y)) != null)
                        {
                            if (distance <= sight * 10)
                            {
                                extraTiles.SetColor(new Vector3Int(Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y), Color.white);
                            }
                        }
                    }
                }
            }
        }
        foreach(GameObject unit in enemyUnits)
        {
            if(unit != null)
            {
                if(unit.GetComponent<SpriteRenderer>() != null)
                {
                    unit.GetComponent<SpriteRenderer>().enabled = false;
                }
                if (unit.GetComponent<UnitStats>().isCorpse)
                {
                    unit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = false;
                }
                else
                {
                    unit.transform.GetChild(2).GetComponent<SpriteMask>().enabled = false;
                }
                Vector3Int pos = new Vector3Int((int)ComFunc.WorldToTileSpace(unit.transform.position).x, (int)ComFunc.WorldToTileSpace(unit.transform.position).y);
                if (floorTiles.GetTile(pos) != null)
                {
                    if (floorTiles.GetColor(pos) == Color.white)
                    {
                        unit.GetComponent<SpriteRenderer>().enabled = true;
                        if (unit.GetComponent<UnitStats>().isCorpse)
                        {
                            unit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = true;
                        }
                        else
                        {
                            unit.transform.GetChild(2).GetComponent<SpriteMask>().enabled = true;
                        }
                        if (!unit.GetComponent<UnitStats>().isCorpse)
                        {
                            unit.transform.GetChild(2).gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (!unit.GetComponent<UnitStats>().isCorpse)
                        {
                            unit.transform.GetChild(2).gameObject.SetActive(false);
                        }
                    }

                    if (unit.GetComponent<UnitStats>().isCorpse)
                    {
                        if (floorTiles.GetColor(pos) != Color.white)
                        {
                            unit.GetComponent<SpriteRenderer>().enabled = false;
                            if (unit.GetComponent<UnitStats>().isCorpse)
                            {
                                unit.transform.GetChild(0).GetComponent<SpriteMask>().enabled = false;
                            }
                            else
                            {
                                unit.transform.GetChild(2).GetComponent<SpriteMask>().enabled = false;
                            }
                            continue;
                        }
                    }
                }
            }
        }
    }
    void Highlights(int stat, GameObject unit, bool needsWalkable, int minStat = 0)
    {
        foreach (GameObject unitA in allyUnits)
        {
            if (unitA != null)
            {
                pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitA.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitA.transform.position).y)].isWalkable = true;
            }
        }
        List<GameObject> hits = new List<GameObject>();
        List<Vector2> skips = new List<Vector2>();
        List<AStarNode> path;
        for (int x = -stat; x < stat; x++)
        {
            for (int y = -stat; y < stat; y++)
            {
                //min range break
                if (x < -minStat && x > minStat) { continue; }
                if (y < -minStat && y > minStat) { continue; }

                if (Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x < 0 || Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y < 0)
                {
                    continue;
                }
                if (x == 0 && y == 0)
                {
                    continue;
                }
                if (needsWalkable)
                {
                    path = pathfinding.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y),
                        Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y);
                }
                else
                {

                    path = temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y),
                        Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y);
                }
                if(path != null)
                {
                    if (path.Count <= minStat)
                    {
                        continue;
                    }
                }
                int layerMask = (1 << 6);
                Vector2 ray = ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) + unit.transform.position;
                RaycastHit2D hit = Physics2D.Raycast(ray, ray, 1, layerMask);

                if (hit.collider != null)
                {
                    int hitX = Mathf.RoundToInt(ComFunc.WorldToTileSpace(hit.collider.transform.position).x);
                    int hitY = Mathf.RoundToInt(ComFunc.WorldToTileSpace(hit.collider.transform.position).y);

                    if (temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y), hitX, hitY).Count <= stat)
                    {
                        if (!hits.Contains(hit.collider.gameObject))
                        {
                            hits.Add(hit.collider.gameObject);
                            skips.Add(new Vector2(hitX, hitY));
                        }
                    }
                }
                
                if (path != null)
                {
                    if (path.Count <= stat)
                    {
                        if (pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y].isWalkable)
                        {
                            List<GameObject> objs = new List<GameObject>();
                            float offsetX = 0;
                            float offsetY = 0;
                            if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y))) == halfTile)
                            {
                                offsetY = -.2f;
                            }

                            if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y)) == halfTile)
                            {
                                if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y)) == halfTile
                                      || floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y)) == null)
                                {
                                    GameObject a = Instantiate(selectL, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, .25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                                    objs.Add(a);

                                    if ((floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == halfTile
                                     || floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == null))
                                    {
                                        GameObject b = Instantiate(selectBL, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, .25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                                        objs.Add(b);
                                    }
                                }
                                if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == halfTile
                                    || floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == null)
                                {
                                    GameObject c = Instantiate(selectR, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, .25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                                    objs.Add(c);

                                    if ((floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == halfTile
                                      || floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x) + x - 1, Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y) + y - 1)) == null))
                                    {
                                        GameObject d = Instantiate(selectBR, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, .25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                                        objs.Add(d);
                                    }
                                }
                                GameObject e = Instantiate(selectT, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, .25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                                objs.Add(e);
                            }
                            else
                            {
                                GameObject f = Instantiate(selectT, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, offsetY), gameObject.transform.rotation, gameObject.transform);
                                GameObject g = Instantiate(selectL, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, offsetY), gameObject.transform.rotation, gameObject.transform);
                                GameObject h = Instantiate(selectBL, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, offsetY), gameObject.transform.rotation, gameObject.transform);
                                GameObject i = Instantiate(selectR, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, offsetY), gameObject.transform.rotation, gameObject.transform);
                                GameObject j = Instantiate(selectBR, unit.transform.position + ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(x, y).x, ComFunc.TileToGridSpace(x, y).y) - new Vector3(offsetX, offsetY), gameObject.transform.rotation, gameObject.transform);

                                objs.Add(f);
                                objs.Add(g);
                                objs.Add(h);
                                objs.Add(i);
                                objs.Add(j);
                            }
                        }
                    }
                }
            }
        }

        if (skips != null)
        {
            foreach (Vector2 v in skips)
            {
                List<GameObject> objs = new List<GameObject>();
                float offsetX = 0;
                float offsetY = 0;

                path = temp.FindPath(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y),
                        (int)v.x, (int)v.y);

                if (path != null)
                {
                    if (path.Count <= stat && floorTiles.GetColor(new Vector3Int((int)v.x, (int)v.y, 0)) == Color.white)
                    {
                        if (floorTiles.GetTile(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y))) == halfTile)
                        {
                            offsetY = 0f;
                        }

                        if (floorTiles.GetTile(new Vector3Int((int)v.x, (int)v.y)) == halfTile)
                        {
                            if (floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y)) == halfTile
                                  || floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y)) == null)
                            {
                                GameObject a = Instantiate(selectL, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, 0 + offsetY), gameObject.transform.rotation, gameObject.transform);
                                objs.Add(a);

                                if ((floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y - 1)) == halfTile
                                 || floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y - 1)) == null))
                                {
                                    GameObject b = Instantiate(selectBL, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, 0 + offsetY), gameObject.transform.rotation, gameObject.transform);
                                    objs.Add(b);
                                }
                            }
                            if (floorTiles.GetTile(new Vector3Int((int)v.x, (int)v.y - 1)) == halfTile
                                || floorTiles.GetTile(new Vector3Int((int)v.x, (int)v.y - 1)) == null)
                            {
                                GameObject c = Instantiate(selectR, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, 0 + offsetY), gameObject.transform.rotation, gameObject.transform);
                                objs.Add(c);

                                if ((floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y - 1)) == halfTile
                                  || floorTiles.GetTile(new Vector3Int((int)v.x - 1, (int)v.y - 1)) == null))
                                {
                                    GameObject d = Instantiate(selectBR, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, 0 + offsetY), gameObject.transform.rotation, gameObject.transform);
                                    objs.Add(d);
                                }
                            }
                            GameObject e = Instantiate(selectT, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, 0 + offsetY), gameObject.transform.rotation, gameObject.transform);
                            objs.Add(e);
                        }
                        else
                        {
                            GameObject f = Instantiate(selectT, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, -.25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                            GameObject g = Instantiate(selectL, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, -.25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                            GameObject h = Instantiate(selectBL, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, -.25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                            GameObject i = Instantiate(selectR, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, -.25f + offsetY), gameObject.transform.rotation, gameObject.transform);
                            GameObject j = Instantiate(selectBR, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y) - new Vector3(offsetX, -.25f + offsetY), gameObject.transform.rotation, gameObject.transform);

                            objs.Add(f);
                            objs.Add(g);
                            objs.Add(h);
                            objs.Add(i);
                            objs.Add(j);
                        }
                    }
                }

                int layerMask = (1 << 6);
                Vector2 ray = ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace((int)v.x, (int)v.y).x, ComFunc.TileToGridSpace((int)v.x, (int)v.y).y);
                RaycastHit2D hit = Physics2D.Raycast(ray, ray, 1, layerMask);

                foreach (GameObject ob in objs)
                {
                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.GetComponent<UnitStats>().isCorpse)
                        {
                            Destroy(ob);
                            continue;
                        }
                        if (hit.collider.gameObject.GetComponent<UnitStats>().isEnemy)
                        {
                            ob.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                        }
                        else
                        {
                            ob.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                        }
                    }
                }
            }
        }
        foreach (GameObject unitA in allyUnits)
        {
            if (unitA != null)
            {
                pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitA.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitA.transform.position).y)].isWalkable = false;
            }
        }
    }
    void ClearHighlights()
    {
        for (int i = 1; i <= gameObject.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }
        for (int i = 0; i < transform.parent.GetChild(1).childCount; i++)
        {
            if (transform.parent.GetChild(1).GetChild(i).GetComponent<Interactable>() != null)
            {
                transform.parent.GetChild(1).GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    void DebuggingTools()
    {
        if (debugSeePathfinding)
        {
            foreach (AStarNode tile in pathfinding.grid)
            {
                if (pathfinding.grid[tile.x, tile.y].isWalkable)
                {
                    floorTiles.SetColor(new Vector3Int(tile.x, tile.y, 0), Color.green);
                }
                else
                {
                    floorTiles.SetColor(new Vector3Int(tile.x, tile.y, 0), Color.red);
                }
            }
            foreach(GameObject unit in enemyUnits)
            {
                if(unit != null)
                {
                    if(unit.GetComponent<SpriteRenderer>() != null)
                    {
                        unit.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }
    GameObject GetUnitAtTile(Vector3Int tilePos)
    {
        int layerMask = (1 << 6);
        Vector2 ray = ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(tilePos.x, tilePos.y).x, ComFunc.TileToGridSpace(tilePos.x, tilePos.y).y) + new Vector3(0,.25f);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(ray, new Vector2(.5f,.5f), 0, ray, 1, layerMask);

        GameObject returnObj = null;
        foreach(RaycastHit2D hit in hits)
        {
            Vector3Int hitPos = new Vector3Int((int)ComFunc.WorldToTileSpace(hit.collider.transform.position).x, (int)ComFunc.WorldToTileSpace(hit.collider.transform.position).y);
            if (floorTiles.GetTile(tilePos) == halfTile)
            {
                //Debug.Log("HalfTile");
                hitPos = new Vector3Int((int)ComFunc.WorldToTileSpace(hit.collider.transform.position).x, Mathf.RoundToInt(ComFunc.WorldToTileSpace(hit.collider.transform.position + new Vector3(0, .25f)).y));
            }

            //Debug.Log(hitPos + "   " + tilePos);
            if (hitPos == tilePos || returnObj == null)
            {
                returnObj = hit.collider.gameObject;
            }
        }
        return returnObj;
    }
    //Moves unit, currently rigid maybe lerp later.
    IEnumerator MoveWaiter(List<AStarNode> path, GameObject unit, bool Enemy = false)
    {
        pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y)].isWalkable = false;
        yield return new WaitForSeconds(.225f);
        bool first = true;
        foreach (AStarNode aStarNode in path)
        {
            Vector2 vec2 = ComFunc.TileToGridSpace(aStarNode.y, aStarNode.x);
            vec2 = new Vector3((vec2.y * -3.5f) + (vec2.x * 3.5f), (vec2.x * 1.75f + (vec2.y * 1.75f)) + .25f, 0);

            if (floorTiles.GetTile(new Vector3Int(aStarNode.x, aStarNode.y)) == halfTile)
            {
                vec2.y = vec2.y - .20f;
            }
            if (unit.GetComponent<UnitStats>().size != 1)
            {
                vec2.x = vec2.x + (.5f * (unit.GetComponent<UnitStats>().size - 1));
            }
            Vector2 unitPosCache = unit.transform.position;
            unit.transform.position = vec2;
            if (!first)
            {
                if (unit.GetComponent<SpriteRenderer>().enabled)
                {
                    Instantiate(stepSound, (Vector3)vec2, transform.rotation);
                }
            }
            first = false;
            if (!Enemy)
            {
                FogUpdate();
            }
            else if(floorTiles.GetColor(new Vector3Int(Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x),Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y))) == Color.white)
            {
                unit.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                unit.GetComponent<SpriteRenderer>().enabled = false;
            }
            pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y)].isWalkable = false;
            pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitPosCache).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitPosCache).y)].isWalkable = true;
            if (!Enemy || (unit.GetComponent<SpriteRenderer>().enabled && Enemy)) { yield return new WaitForSeconds(.225f); }
        }
        pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unit.transform.position).y)].isWalkable = false;
    }
    IEnumerator AIHelper(GameObject unit)
    {
        //Spread AI units out across several frames
        yield return new WaitForSeconds(Random.Range(0f, .3f));
        bool didSomething = false;
        UnitStats stats = unit.GetComponent<UnitStats>();
        int sight = stats.sightRange;
        List<AStarNode> path;
        int loops = 0;

        //Each action point update vision and choose a behavior based on that information. 
        while (stats.actionPointCurrent > 0)
        {
            if(loops > 6) { break; }
            loops++;
            Vector2 unitPos = ComFunc.WorldToTileSpace(unit.transform.position);
            List<GameObject> seenPlayerUnits = new List<GameObject>();
            List<AStarNode> seenTiles = new List<AStarNode>();
            //Sight
            for (int x = -sight + 2; x <= sight - 2; x++)
            {
                for (int y = -sight + 2; y <= sight - 2; y++)
                {
                    if (Mathf.RoundToInt(unitPos.x) + x < 0 || Mathf.RoundToInt(unitPos.y) + y < 0
                        || x + unitPos.x > (int)maker.MapSize * 7 + 2 || y + unitPos.y > (int)maker.MapSize * 7 + 2) { continue; }

                    path = temp.FindPath(Mathf.RoundToInt(unitPos.x), Mathf.RoundToInt(unitPos.y), Mathf.RoundToInt(unitPos.x) + x, Mathf.RoundToInt(unitPos.y) + y);
                    if (path != null)
                    {
                        if (path.Count <= sight)
                        {
                            seenTiles.Add(path[path.Count - 1]);
                            int layerMask = (1 << 6);
                            Vector2 ray = ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(path[path.Count - 1].x, path[path.Count - 1].y).x, ComFunc.TileToGridSpace(path[path.Count - 1].x, path[path.Count - 1].y).y);
                            RaycastHit2D hit = Physics2D.Raycast(ray, ray, 1, layerMask);
                            if (hit.collider != null)
                            {
                                if (!hit.collider.gameObject.GetComponent<UnitStats>().isEnemy)
                                {
                                    seenPlayerUnits.Add(hit.collider.gameObject);
                                }
                            }
                        }
                    }
                }
            }
            //Chase & Attack Behavior
            if(seenPlayerUnits.Count > 0)
            {
                //Debug.Log("Can see player units");
                List<GameObject> unitsInRange1 = new List<GameObject>();
                List<GameObject> unitsInRange2 = new List<GameObject>();
                foreach (GameObject playerUnit in seenPlayerUnits)
                {
                    Vector2 playerUnitPos = ComFunc.WorldToTileSpace(playerUnit.transform.position);
                    pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = true;
                    pathfinding.grid[(int)playerUnitPos.x, (int)playerUnitPos.y].isWalkable = true;
                    path = pathfinding.FindPath((int)unitPos.x, (int)unitPos.y, (int)playerUnitPos.x, (int)playerUnitPos.y);
                    pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = false;
                    pathfinding.grid[(int)playerUnitPos.x, (int)playerUnitPos.y].isWalkable = false;
                    if (path != null)
                    {
                        if (path.Count <= stats.action1.range && stats.action1.targetingType == 0 && path.Count >= stats.action1.minRange)
                        {
                            unitsInRange1.Add(playerUnit);
                            //Debug.Log("Units in range 1");
                        }
                        if(path.Count <= stats.action2.range && stats.action1.targetingType == 0 && path.Count >= stats.action2.minRange)
                        {
                            unitsInRange2.Add(playerUnit);
                            //Debug.Log("Units in range 2");
                        }
                    }
                }
                if ((unitsInRange1.Count > 0 || unitsInRange2.Count > 0) && (stats.actionPointCurrent >= stats.action1.AP || stats.actionPointCurrent >= stats.action2.AP))
                {
                    //Debug.Log("Entered Attack Behavior");
                    //  Attack Behavior
                    ActionClass action = null;
                    GameObject unitToAttack = null;
                    if(unitsInRange1.Count > 0 && unitsInRange2.Count > 0 && stats.actionPointCurrent>= stats.action1.AP && stats.actionPointCurrent >= stats.action2.AP)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            action = stats.action1;
                            unitToAttack = unitsInRange1[Random.Range(0, unitsInRange1.Count)];
                        }
                        else
                        {
                            action = stats.action2;
                            unitToAttack = unitsInRange2[Random.Range(0, unitsInRange2.Count)];
                        }
                    }
                    else if(unitsInRange1.Count > 0 && stats.actionPointCurrent >= stats.action1.AP)
                    {
                        action = stats.action1;
                        unitToAttack = unitsInRange1[Random.Range(0, unitsInRange1.Count)];
                    }
                    else if(unitsInRange2.Count > 0 && stats.actionPointCurrent >= stats.action2.AP)
                    {
                        action = stats.action2;
                        unitToAttack = unitsInRange2[Random.Range(0, unitsInRange2.Count)];
                    }
                    if(unitToAttack != null)
                    {
                        cameraSS.start = true;
                        int damage = Mathf.RoundToInt(stats.attack * action.damageMult) - Random.Range(-1, 2);
                        unitToAttack.GetComponent<UnitStats>().hpCurrent -= damage;
                        StartCoroutine(CheckEffects(unit, action, unitToAttack));
                        if (unitToAttack.GetComponent<UnitStats>().hpCurrent <= 0)
                        {
                            if (!unitToAttack.GetComponent<UnitStats>().isCorpse && unitToAttack.GetComponent<UnitStats>().spawnsCorpse)
                            {
                                GameObject a = Instantiate(corpse, unitToAttack.transform.position, unitToAttack.transform.rotation, transform.parent.GetChild(1));
                                enemyUnits.Add(a);
                                AIsDone++;
                                FogUpdate();
                            }
                            else
                            {
                                pathfinding.grid[Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).x), Mathf.RoundToInt(ComFunc.WorldToTileSpace(unitToAttack.transform.position).y)].isWalkable = true;
                            }
                            StartCoroutine(CheckPlayerUnits());
                            Destroy(unitToAttack);
                        }
                        if (action.summon)
                        {
                            List<AStarNode> tilesInRange = new List<AStarNode>();
                            Vector3 unitToAttackPos = ComFunc.WorldToTileSpace(unitToAttack.transform.position);
                            for (int x = -2; x < 3; x++)
                            {
                                for (int y = -2; y < 3; y++)
                                {
                                    if (Mathf.RoundToInt(unitPos.x) + x < 0 || Mathf.RoundToInt(unitPos.y) + y < 0
                                    || x + unitPos.x > (int)maker.MapSize * 7 + 2 || y + unitPos.y > (int)maker.MapSize * 7 + 2) { continue; }

                                    if (pathfinding.grid[Mathf.RoundToInt(unitToAttackPos.x) + x, Mathf.RoundToInt(unitToAttackPos.y) + y].isWalkable)
                                    {
                                        tilesInRange.Add(pathfinding.grid[Mathf.RoundToInt(unitToAttackPos.x) + x, Mathf.RoundToInt(unitToAttackPos.y) + y]);
                                    }
                                }
                            }
                            if (tilesInRange.Count != 0)
                            {
                                AStarNode targetTile = tilesInRange[Random.Range(0, tilesInRange.Count)];
                                StartCoroutine(CheckEffects(unit, action, targetTile: targetTile));
                                pathfinding.grid[targetTile.x, targetTile.y].isWalkable = false;
                                AIsDone++;
                            }
                        }
                        else
                        {
                            GameObject b = Instantiate(damageNumber, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                            b.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
                            Instantiate(action.animationPrefab, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                            Instantiate(action.soundPrefab, unitToAttack.transform.position + new Vector3(0, .25f), unitToAttack.transform.rotation);
                        }
                        stats.actionPointCurrent -= action.AP;
                        didSomething = true;
                        yield return new WaitForSeconds(.75f);
                    }
                    else
                    {
                        stats.actionPointCurrent--;
                    }
                }
                else if(stats.speed <= 0)
                {
                    stats.actionPointCurrent = 0;
                }
                else
                {
                    // Chase Behavior
                    List<AStarNode> tempPath = new List<AStarNode>();
                    GameObject closestUnit = null;
                    foreach (GameObject seenUnit in seenPlayerUnits)
                    {
                        Vector2 seenUnitPos = ComFunc.WorldToTileSpace(seenUnit.transform.position);
                        pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = true;
                        path = pathfinding.FindPath((int)unitPos.x, (int)unitPos.y, (int)seenUnitPos.x, (int)seenUnitPos.y);
                        pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = false;
                        if (closestUnit == null || tempPath == null)
                        {
                            closestUnit = seenUnit;
                            tempPath = path;
                        }
                        else if(tempPath.Count > path.Count)
                        {
                            closestUnit = seenUnit;
                            tempPath = path;
                        }
                    }
                    GameObject targetUnit = closestUnit;
                    Vector2 targetUnitPos = ComFunc.WorldToTileSpace(targetUnit.transform.position);

                    path = null;
                    int attempts = 0;
                    List<AStarNode> shortestPath = new List<AStarNode>();
                    while (path == null)
                    {
                        shortestPath = null;
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = true;
                                path = pathfinding.FindPath((int)unitPos.x, (int)unitPos.y, Mathf.Clamp(0, (int)targetUnitPos.x + x, pathfinding.grid.Length), (int)Mathf.Clamp(0, (int)targetUnitPos.y + y, pathfinding.grid.LongLength));
                                pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = false;
                                if (path != null)
                                {
                                    if (shortestPath == null)
                                    {
                                        shortestPath = path;
                                    }
                                    else if (shortestPath.Count > path.Count)
                                    {
                                        shortestPath = path;
                                    }
                                }
                            }
                        }
                        attempts++;
                        if(attempts > 10)
                        {
                            path = null;
                            break;
                        }
                    }

                    if (shortestPath != null)
                    {
                        while(shortestPath.Count > stats.speed)
                        {
                            if(shortestPath.Count > stats.action1.range && shortestPath.Count > stats.action2.range)
                            {
                                shortestPath.Remove(shortestPath[shortestPath.Count - 1]);
                            }
                            else
                            {
                                break;
                            }
                        }
                        pathfinding.grid[shortestPath[shortestPath.Count - 1].x, shortestPath[shortestPath.Count - 1].y].isWalkable = false;
                        StartCoroutine(MoveWaiter(shortestPath, unit, true));
                        stats.actionPointCurrent--;
                        didSomething = true;
                        yield return new WaitForSeconds(.225f * (shortestPath.Count + 1));
                    }
                }
            }
            //Wander Behavior
            else
            {
                bool summoned = false;
                /* Keeping this mainly for AI scout effects later
                if((stats.actionPointCurrent > stats.action1.AP && stats.action1.AIUseWhenWander) || (stats.actionPointCurrent > stats.action2.AP && stats.action2.AIUseWhenWander))
                {
                    if(Random.Range(0,2) == 1)
                    {
                        List<AStarNode> tilesInRange = new List<AStarNode>();
                        foreach(AStarNode tile in seenTiles)
                        {
                            if(temp.FindPath(Mathf.RoundToInt(unitPos.x), Mathf.RoundToInt(unitPos.y),tile.x,tile.y).Count <= stats.action2.range)
                            {
                                tilesInRange.Add(tile);
                            }
                        }
                        if (stats.action1.summon && stats.actionPointCurrent > stats.action1.AP)
                        {
                            StartCoroutine(CheckEffects(unit, stats.action1, targetTile: tilesInRange[Random.Range(0, tilesInRange.Count)]));
                            stats.actionPointCurrent -= stats.action2.AP;
                            AIsDone++;
                            summoned = true;
                        }
                        else if (stats.action2.summon && stats.actionPointCurrent > stats.action2.AP)
                        {
                            StartCoroutine(CheckEffects(unit, stats.action2, targetTile: tilesInRange[Random.Range(0, tilesInRange.Count)]));
                            stats.actionPointCurrent -= stats.action2.AP;
                            AIsDone++;
                            summoned = true;
                        }
                    }
                }
                */
                if (!summoned)
                {
                    path = null;
                    int attempts = 0;
                    while (path == null)
                    {
                        int randomX = (int)Mathf.Clamp(Random.Range(-stats.speed, stats.speed) + (int)unitPos.x, 0, pathfinding.grid.Length);

                        int randomY = (int)Mathf.Clamp(Random.Range(-stats.speed, stats.speed) + (int)unitPos.y, 0, pathfinding.grid.LongLength);
                        pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = true;
                        path = pathfinding.FindPath((int)unitPos.x, (int)unitPos.y, randomX, randomY);
                        pathfinding.grid[(int)unitPos.x, (int)unitPos.y].isWalkable = false;
                        attempts++;
                        if (attempts > 100)
                        {
                            break;
                        }
                    }
                    if (path != null)
                    {
                        pathfinding.grid[path[path.Count - 1].x, path[path.Count - 1].y].isWalkable = false;
                        StartCoroutine(MoveWaiter(path, unit, true));
                        yield return new WaitForSeconds(.225f * (path.Count + 1));
                    }
                    stats.actionPointCurrent--;
                    didSomething = true;
                }
            }
            if (!didSomething)
            {
                stats.actionPointCurrent--;
                yield return new WaitForSeconds(.1f);
            }
        }
        AIsDone++;
        stats.actionPointCurrent = stats.actionPointMax;
    }
    IEnumerator CheckEffects(GameObject castingUnit, ActionClass action, GameObject targetUnit = null, AStarNode targetTile = null)
    {
        if(targetUnit != null)
        {
            if (action.stun)
            {
                targetUnit.GetComponent<UnitStats>().actionPointCurrent -= 3;
            }
        }
        if(targetTile != null)
        {
            if (action.summon)
            {
                GameObject effect = Instantiate(action.animationPrefab, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(targetTile.x, targetTile.y).x, ComFunc.TileToGridSpace(targetTile.x, targetTile.y).y) + new Vector3(0, .25f), transform.rotation, transform.parent.GetChild(1));
                Instantiate(action.soundPrefab, ComFunc.GridToWorldSpace(ComFunc.TileToGridSpace(targetTile.x, targetTile.y).x, ComFunc.TileToGridSpace(targetTile.x, targetTile.y).y), transform.rotation);
                if (floorTiles.GetTile(new Vector3Int(targetTile.x, targetTile.y)) == halfTile)
                {
                    effect.transform.position -= new Vector3(0,.25f);
                }
                if (castingUnit.GetComponent<UnitStats>().isEnemy)
                {
                    enemyUnits.Add(effect);
                    effect.GetComponent<UnitStats>().isEnemy = true;
                }
                else
                {
                    allyUnits.Add(effect);
                    effect.GetComponent<UnitStats>().isEnemy = false;
                }
                FogUpdate();
            }
        }
        yield return new WaitForSeconds(.05f);
    }
    IEnumerator CheckPlayerUnits()
    {
        yield return new WaitForSeconds(.25f);

        List<GameObject> removes = new List<GameObject>();
        foreach(GameObject ob in allyUnits)
        {
            if (ob == null)
            {
                removes.Add(ob);
            }
            else if (ob.GetComponent<UnitStats>().isCorpse)
            {
                removes.Add(ob);
            }
        }
        foreach (GameObject ob in removes)
        {
            allyUnits.Remove(ob);
        }

        yield return new WaitForSeconds(.05f);
        if (allyUnits.Count <= 0)
        {
            gameOverFadeIn.GetComponent<UIColorFade>().Trigger();
            yield return new WaitForSeconds(.75f);
            gameOver.SetActive(true);
        }
        /*foreach(GameObject ob in allyUnits)
        {
            Debug.Log(ob);
        }*/
    }
}