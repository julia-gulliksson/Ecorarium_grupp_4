using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceStateManager : MonoBehaviour
{
    FenceBaseState currentState;
    public FenceDamageState DamageState = new FenceDamageState();
    public FenceRepairableState RepairableState = new FenceRepairableState();

    [SerializeField] public int side;
    private int baseHealth = 100;
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int DamagedHealth { get; private set; }

    [System.NonSerialized] public Coroutine resetHealth;
    [System.NonSerialized] public Coroutine repairHealth;
    [System.NonSerialized] public Coroutine damageHealth;

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
        MaxHealth = baseHealth * children;
        Health = MaxHealth;
        currentState = DamageState;

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
        Debug.Log("Success! Selected");
    }

    public void OnDeSelectFence()
    {
        Debug.Log("Deselected!");
    }
}
