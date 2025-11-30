using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class Light2DVaryIntensity : MonoBehaviour
{
    public float minIntensity;
    public float maxIntenisty;
    // starting value for the Lerp
    static float t = 1.0f;
    public new Light2D light;
    bool flipFlop = true;
    float randomTime = .5f;

    void Update()
    {
        // animate the position of the game object...
        light.intensity = Mathf.Lerp(minIntensity, maxIntenisty, t);

        // .. and increase the t interpolater
        if (flipFlop)
        {
            t += .5f * randomTime * Time.deltaTime;
        }
        else
        {
            t -= .5f * randomTime * Time.deltaTime;
        }

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f && flipFlop || t < 0f && !flipFlop)
        {
            if (flipFlop)
            {
                randomTime = UnityEngine.Random.Range(.2f, .7f);
            }
            flipFlop = !flipFlop;
        }
    }
}
