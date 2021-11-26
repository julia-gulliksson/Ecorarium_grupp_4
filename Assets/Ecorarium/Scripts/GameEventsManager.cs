using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action<bool> onWolfCollide;

    void Awake()
    {
        current = this;
    }

    public void WolfFoundTarget(bool found)
    {
        onWolfCollide?.Invoke(found);
    }
}
