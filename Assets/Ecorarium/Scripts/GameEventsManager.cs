using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action onWolfFoundTarget;
    public event Action onWolfLostTarget;
    public event Action<List<Vector3>> onTargetPointsGenerated;

    public event Action OnDay;
    public event Action OnNight;

    void Awake()
    {
        current = this;
    }

    public void TargetPointsGenerated(List<Vector3> targetPoints)
    {
        onTargetPointsGenerated?.Invoke(targetPoints);
    }

    public void WolfFoundTarget()
    {
        onWolfFoundTarget?.Invoke();
    }

    public void WolfLostTarget()
    {
        onWolfLostTarget?.Invoke();
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
