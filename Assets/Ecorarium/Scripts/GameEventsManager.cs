using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action onWolfFoundTarget;
    public event Action onWolfLostTarget;

    void Awake()
    {
        current = this;
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
