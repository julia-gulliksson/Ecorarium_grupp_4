using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action<int> onWolfFoundTarget;
    public event Action<int> onWolfLostTarget;
    public event Action<int, float> onFenceHealthChanged;

    public event Action OnDay;
    public event Action OnNight;

    void Awake()
    {
        current = this;
    }

    public void WolfFoundTarget(int fenceSide)
    {
        onWolfFoundTarget?.Invoke(fenceSide);
    }

    public void WolfLostTarget(int fenceSide)
    {
        onWolfLostTarget?.Invoke(fenceSide);
    }

    public void FenceHealthChanged(int fenceSide, float healthPercentage)
    {
        onFenceHealthChanged?.Invoke(fenceSide, healthPercentage);
    }

    public void Day()
    {
        OnDay?.Invoke();
    }

    public void Night()
    {
        OnNight?.Invoke();
    }

}
