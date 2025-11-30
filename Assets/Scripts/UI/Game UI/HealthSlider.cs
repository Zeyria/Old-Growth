using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider slider;
    private UnitStats unitstats;
    private void Start()
    {
        unitstats = this.GetComponentInParent<UnitStats>();
        slider.value = unitstats.hpCurrent;
    }
    private void Update()
    {
        slider.value = unitstats.hpCurrent;
    }
}
