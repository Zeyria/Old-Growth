using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSFXOnStart : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<AudioSource>().volume = Config.SFXVolume;
    }
}
