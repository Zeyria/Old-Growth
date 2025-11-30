using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAfterTime : MonoBehaviour
{
    public float timeSec;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(timeSec);
        Destroy(gameObject);
    }
}
