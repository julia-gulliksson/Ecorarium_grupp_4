using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager current;

    public event Action<bool,int> onWolfCollide;

    void Awake()
    {
        current = this;
    }

    public void WolfFoundTarget(bool found, int id)
    {
        onWolfCollide?.Invoke(found, id);
    }

}
