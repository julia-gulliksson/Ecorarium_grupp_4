using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceStateManager : MonoBehaviour
{
    FenceBaseState currentState;
    public FenceDamageState DamageState = new FenceDamageState();
    public FenceRepairState RepairState = new FenceRepairState();
    public FenceResetState ResetState = new FenceResetState();

    [SerializeField] public int side;
    [SerializeField] LayerMask fenceMask;
    private int baseHealth = 100;
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int DamagedHealth { get; private set; }
    public bool IsNight { get; private set; }

    [System.NonSerialized] public Coroutine resetHealth;
    [System.NonSerialized] public Coroutine repairHealth;
    [System.NonSerialized] public Coroutine damageHealth;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfFoundTarget += DamageState.IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget += DamageState.DecrementWolvesAttacking;
        GameEventsManager.current.OnNight += HandleNight;
        GameEventsManager.current.OnDay += HandleDay;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfFoundTarget -= DamageState.IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget -= DamageState.DecrementWolvesAttacking;
        GameEventsManager.current.OnDay -= HandleNight;
        GameEventsManager.current.OnDay -= HandleDay;
    }

    void Start()
    {
        int children = 0;
        foreach (Transform childTransform in transform)
        {
            if (fenceMask == (fenceMask | (1 << childTransform.gameObject.layer)))
            {
                // Add all the fence assets (with layer that matches the fenceMask)
                children++;
            }
        }
        // Base health on amount of fence assets this side has
        MaxHealth = baseHealth * children;
        Health = MaxHealth;

        currentState = DamageState;
        currentState.EnterState(this);
    }

    public void SwitchState(FenceBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState(this);
    }

    private void HandleNight()
    {
        IsNight = true;
        if (currentState != ResetState && !MaxHealthReached())
        {
            SwitchState(ResetState);
        }
        else if (currentState != DamageState)
        {
            SwitchState(DamageState);
        }
    }

    private void HandleDay()
    {
        IsNight = false;
    }

    public void UpdateHealth(int updatedHealth)
    {
        Health = updatedHealth;
    }

    public void SetDamagedHealth(int updatedHealth)
    {
        DamagedHealth = updatedHealth;
    }

    public void SendUpdatedHealth(int updatedHealth)
    {
        float healthPercentage = ((float)updatedHealth / (float)MaxHealth) * 100;
        GameEventsManager.current.FenceHealthChanged(side, healthPercentage);
    }

    public void DestroyFence()
    {
        GameEventsManager.current.FenceBroke();
        Destroy(gameObject);
    }

    public void OnSelectFence()
    {
        if (!IsNight && !MaxHealthReached())
        {
            SwitchState(RepairState);
        }
    }

    public void OnDeSelectFence()
    {
        if (!IsNight && !MaxHealthReached())
        {
            SwitchState(ResetState);
        }
    }

    public bool MaxHealthReached()
    {
        return Health >= MaxHealth;
    }
}
