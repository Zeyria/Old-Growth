using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUI : MonoBehaviour
{
    public List<GameObject> objs;
    private UnitStats parStats;
    private void Awake()
    {
        parStats = GetComponentInParent<UnitStats>();
        Update();
    }
    private void Update()
    {
        if (parStats.isEnemy)
        {
            foreach(GameObject gameObject in objs)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
