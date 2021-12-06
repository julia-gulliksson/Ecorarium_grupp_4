public abstract class WolfBaseState
{
    public abstract void EnterState(WolfStateManager wolf);
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void OnDestroy();
}
