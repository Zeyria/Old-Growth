using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSyncToggle : MonoBehaviour
{
    public Toggle vtoggle;
    private void Start()
    {
        vtoggle.isOn = Config.VSync;
        ToggleCheck();
        vtoggle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    void ValueChangeCheck()
    {
        Config.VSync = vtoggle.isOn;
        ToggleCheck();
    }
    void ToggleCheck()
    {
        if (vtoggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = 144;
        }
    }
}
