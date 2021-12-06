using UnityEngine;

public abstract class WolfBaseState
{
    public abstract void EnterState(WolfStateManager wolf);
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnDestroy();
    public abstract void OnTriggerEnter(Collider collider);
}
