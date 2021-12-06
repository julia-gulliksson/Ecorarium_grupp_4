using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfStateManager : MonoBehaviour
{
    WolfBaseState currentState;

    public WolfAttackFenceState AttackFenceState = new WolfAttackFenceState();
    public WolfAttackSheepState AttackSheepState = new WolfAttackSheepState();
    public WolfRetreatState RetreatState = new WolfRetreatState();

    public IFence fenceScript;
    public Vector3 targetPoint;
    [SerializeField] public LayerMask hitMask;
    public NavMeshAgent navMeshAgent;
    public int id;

    private void OnEnable()
    {
        GameEventsManager.current.onFenceBreak += AttackFenceState.HandleFenceBreak;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onFenceBreak -= AttackFenceState.HandleFenceBreak;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        currentState = AttackFenceState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(WolfBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void OnDestroy()
    {
        currentState.OnDestroy();
    }
}
