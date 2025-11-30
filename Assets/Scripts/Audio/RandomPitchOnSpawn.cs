using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchOnSpawn : MonoBehaviour
{
    void Start()
    {
        float pitch = this.GetComponent<AudioSource>().pitch;
        this.GetComponent<AudioSource>().pitch = Random.Range(pitch - .2f, pitch + .2f);
    }
}
