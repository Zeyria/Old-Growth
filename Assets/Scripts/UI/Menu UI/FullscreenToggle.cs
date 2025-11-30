using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle ftoggle;
    private void Start()
    {
        ftoggle.isOn = Config.Fullscreen;
        ToggleCheck();
        ftoggle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    void ValueChangeCheck()
    {
        Config.Fullscreen = ftoggle.isOn;
        ToggleCheck();
    }
    void ToggleCheck()
    {
        Screen.fullScreen = Config.Fullscreen;
    }
}
