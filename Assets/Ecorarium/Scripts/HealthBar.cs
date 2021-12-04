using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    int fenceSide;
    Image barImage;

    private void Start()
    {
        barImage = GetComponent<Image>();
        try
        {
            fenceSide = transform.parent.parent.GetComponent<FenceHealth>().side;
        }
        catch
        {
            fenceSide = 0;
        }
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
            barImage.fillAmount = healthPercentage / 100;
        }
    }
}
