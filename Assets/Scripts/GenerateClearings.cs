using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateClearings : MonoBehaviour
{
    public int numClearings;
    public GameObject clearingObj;
    public GameObject holder;
    private void Start()
    {
        ArenaSettings.hasActiveMap = false;
        ArenaSettings.activeMapSettings = null;
        for (int i = 1; i <= numClearings; i++)
        {
            int selectSpawnPoint = Random.Range(1, transform.childCount);
            Instantiate(clearingObj, transform.GetChild(selectSpawnPoint).position, transform.rotation, transform.GetChild(0));
            transform.GetChild(selectSpawnPoint).SetParent(holder.transform);
        }
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(holder.transform);
        }
    }
}
