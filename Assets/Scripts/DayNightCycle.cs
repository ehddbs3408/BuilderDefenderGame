using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float secondsPerday = 10f;
    [SerializeField] private Gradient gradient;

    private float daytime;
    private float dayTimeSpeed;

    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        dayTimeSpeed = 1 / secondsPerday;
    }

    private void Update()
    {
        daytime += Time.deltaTime * dayTimeSpeed;
        light2D.color = gradient.Evaluate(daytime % 1f);
    }

}
