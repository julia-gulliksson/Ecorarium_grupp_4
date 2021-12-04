using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] int fenceSide;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        GameEventsManager.current.onFenceHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onFenceHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int side, float healthPercentage)
    {
        if (side == fenceSide)
        {
            slider.value = healthPercentage;
        }
    }
}
