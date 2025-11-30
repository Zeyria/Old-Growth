using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{
    private void Awake()
    {
        Config.musicVolume = PlayerPrefs.GetFloat("musicVolume", .5f);
        Config.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", .5f);
    }
}
