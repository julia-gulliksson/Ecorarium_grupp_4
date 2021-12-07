using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceStateManager : MonoBehaviour
{
    FenceBaseState currentState;
    public FenceDamageState DamageState = new FenceDamageState();
    public FenceRepairState RepairState = new FenceRepairState();
    public FenceResetState ResetState = new FenceResetState();
    public FenceRepairableState RepairableState = new FenceRepairableState();

    [System.NonSerialized] public int baseHealth = 100;
    [System.NonSerialized] public int health;
    [System.NonSerialized] public int maxHealth;
    [System.NonSerialized] public int damagedHealth;

    [SerializeField] public int side;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfFoundTarget += DamageState.IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget += DamageState.DecrementWolvesAttacking;
        GameEventsManager.current.OnDay += HandleDay;
        GameEventsManager.current.OnNight += HandleNight;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfFoundTarget -= DamageState.IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget -= DamageState.DecrementWolvesAttacking;
        GameEventsManager.current.OnDay -= HandleDay;
        GameEventsManager.current.OnNight -= HandleNight;
    }


    void Start()
    {
        int children = 0;
        foreach (Transform childTransform in transform)
        {
            if (childTransform.name.Contains("Fence")) children++;
        }
        // Base health on amount of fence assets this side has
        maxHealth = baseHealth * children;
        health = maxHealth;

        currentState = RepairableState;
        currentState.EnterState(this);

    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(FenceBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState(this);
    }

    void HandleDay()
    {
        SwitchState(RepairableState);
    }

    void HandleNight()
    {
        if (currentState != DamageState)
        {
            SwitchState(DamageState);
        }
    }
}
