using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    public float speed;
    Vector3 dir;
    private void Start()
    {
        dir = new Vector3(Random.Range(.5f, 1f), Random.Range(.5f, 1f), 0);
        speed = Random.Range(speed * .8f, speed * 1.2f);
    }
    private void Update()
    {
        this.transform.position += dir * speed * Time.deltaTime;
    }
}
