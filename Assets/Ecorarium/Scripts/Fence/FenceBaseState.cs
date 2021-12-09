using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FenceBaseState
{
    public abstract void EnterState(FenceStateManager fence);
    public abstract void ExitState();
}
