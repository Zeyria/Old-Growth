using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class DontDestroyOnLoadManager
{
    public static List<GameObject> _ddolObjects = new List<GameObject>();

    public static void DontDestroyOnLoad(this GameObject go)
    {
        UnityEngine.Object.DontDestroyOnLoad(go);
        _ddolObjects.Add(go);
    }

    public static void DestroyAll()
    {
        foreach (var go in _ddolObjects)
            if (go != null)
                UnityEngine.Object.Destroy(go);

        _ddolObjects.Clear();
    }
}
public class ArmyBuilder : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        if (DDTracker.hasAG)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            gameObject.DontDestroyOnLoad();
            DDTracker.hasAG = true;
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Town"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
    }
}
