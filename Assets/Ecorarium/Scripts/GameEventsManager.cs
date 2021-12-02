using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action onWolfFoundTarget;
    public event Action onWolfLostTarget;
    public event Action<List<Vector3>> onTargetPointsGenerated;

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
}
