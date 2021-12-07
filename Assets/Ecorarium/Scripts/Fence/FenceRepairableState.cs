using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceRepairableState : FenceBaseState
{
    FenceStateManager fence;

    public override void EnterState(FenceStateManager fenceRef)
    {
        fence = fenceRef;
        Debug.Log("Hello from repairable state");
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(0))
        {
            fence.SwitchState(fence.RepairState);
        }
        else
        {
            fence.SwitchState(fence.ResetState);
        }
    }

    public override void ExitState()
    {
    }
}
