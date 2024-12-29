using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightOscillator : MonoBehaviour
{
    private Light2D light2D;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        light2D.intensity = 0.7f + 0.1f * Mathf.Sin(Time.time * Mathf.PI);
    }
}
