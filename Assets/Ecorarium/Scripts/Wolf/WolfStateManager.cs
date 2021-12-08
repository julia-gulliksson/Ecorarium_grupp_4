using UnityEngine;
using UnityEngine.AI;

public class WolfStateManager : MonoBehaviour, IDestroyable
{
    WolfBaseState currentState;

    public WolfAttackFenceState AttackFenceState = new WolfAttackFenceState();
    public WolfAttackSheepState AttackSheepState = new WolfAttackSheepState();
    public WolfRetreatState RetreatState = new WolfRetreatState();

    public IFence fenceScript;
    public LayerMask hitMask;
    public NavMeshAgent navMeshAgent;
    public int id;
    bool fenceHasBroken = false;
    [System.NonSerialized] public Vector3 targetPoint;
    [System.NonSerialized] public Vector3 spawnPosition;
    int baseHealth = 400;

    private void OnEnable()
    {
        GameEventsManager.current.onFenceBreak += AttackFenceState.HandleFenceBreak;
        GameEventsManager.current.onFenceBreak += HandleFenceBreak;
        GameEventsManager.current.OnDay += HandleDay;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onFenceBreak -= AttackFenceState.HandleFenceBreak;
        GameEventsManager.current.onFenceBreak -= HandleFenceBreak;
        GameEventsManager.current.OnDay -= HandleDay;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        currentState = AttackFenceState;
        currentState.EnterState(this);
        spawnPosition = transform.position;
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(WolfBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState(this);
    }

    private void OnDestroy()
    {
        currentState.OnDestroy();
    }
    private void OnTriggerEnter(Collider collider)
    {
        currentState.OnTriggerEnter(collider);
    }

    void HandleDay()
    {
        if (!fenceHasBroken)
        {
            // Wolves should only retreat if a fence is not broken
            SwitchState(RetreatState);
        }
    }

    void HandleFenceBreak()
    {
        fenceHasBroken = true;
    }

    public void Damage()
    {
        baseHealth -= 100;
        if (baseHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
