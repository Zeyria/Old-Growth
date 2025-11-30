using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDTracker
{
    public static bool hasMM = false;
    public static bool hasAG = false;
}
public class SetMusicVolume : MonoBehaviour
{
    public AudioSource music;
    public AudioClip[] clips;
    public string[] scenes;
    private bool trans;
    private bool trans2;
    private void Start()
    {
        trans = false;
        trans2 = false;
        if (DDTracker.hasMM)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            gameObject.DontDestroyOnLoad();
            DDTracker.hasMM = true;
        }

    }
    void Update()
    {
        /*
        if (!trans)
        {
            music.volume = Config.musicVolume;
        }
        */
        music.volume = Config.musicVolume;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(scenes[0]) && music.clip != clips[0])
        {
            transMusic(clips[0]);
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(scenes[1]) && music.clip != clips[0])
        {
            transMusic(clips[0]);
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(scenes[2]) && music.clip != clips[1])
        {
            transMusic(clips[1]);
        }
        if(music.volume < Config.musicVolume && trans && trans2)
        {
            transMusic(music.clip);
        }
    }
    void transMusic(AudioClip a)
    {
        /*
        trans = true;
        if(music.volume > 0 && music.clip != a)
        {
            music.volume -= .01f;
            return;
        }
        */
        if(music.clip != a)
        {
            music.clip = a;
            music.Play();
        }
        /*
        trans2 = true;
        if (music.volume < Config.musicVolume)
        {
            music.volume += .01f;
            return;
        }
        trans = false;
        trans2 = false;
        */
    }
}
