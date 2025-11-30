using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public ObjAndIntClass[] enemiesAndSpawnChance;
    private GameObject map;
    public bool addToEnemyUnits = false;
    private void Start()
    {
        map = GameObject.Find("Map");
        StartCoroutine(AfterUpdates());
    }
    IEnumerator AfterUpdates()
    {
        int totalChance = 0;
        System.Random prng = new System.Random(Time.time.GetHashCode() + map.transform.childCount);
        foreach(ObjAndIntClass objint in enemiesAndSpawnChance)
        {
            totalChance += objint.nt;
        }
        int prngInt = prng.Next(1, totalChance + 1);
        int tempInt = 0;
        int loopCount = 0;
        while(prngInt > tempInt)
        {
            loopCount++;
            tempInt += enemiesAndSpawnChance[loopCount - 1].nt;
        }
        GameObject obj = Instantiate(enemiesAndSpawnChance[loopCount-1].obj, this.transform.position, this.transform.rotation);
        obj.transform.SetParent(map.transform);
        if (obj.TryGetComponent<UnitStats>(out UnitStats unitStats))
        {
            unitStats.hpCurrent = unitStats.hpMax;
        }
        if (addToEnemyUnits)
        {
            map.transform.parent.GetChild(0).GetComponent<UnitControler>().enemyUnits.Add(obj);
        }
        yield return new WaitForSeconds(.01f);
        Destroy(this.gameObject);
    }
}
