using System.Collections;
using UnityEngine;

public class FenceRepairState : FenceBaseState
{
    FenceStateManager fence;
    float repairSpeed = 0.1f;

    public override void EnterState(FenceStateManager fenceRef)
    {
        //fence = fenceRef;
        //Debug.Log("Hello from repair state");
        //fence.StartCoroutine(Repair());
    }

    public override void UpdateState()
    {
        //if (!Input.GetMouseButton(0))
        //{
        //    fence.SwitchState(fence.ResetState);
        //}
    }

    public override void ExitState()
    {
        //fence.StopCoroutine(Repair());
    }

    //IEnumerator Repair()
    //{
    //    while (fence.health < fence.maxHealth)
    //    {
    //        fence.health++;
    //        if (fence.side == 1)
    //        {
    //            Debug.Log("Repairing! " + fence.health);

    //        }
    //        float healthPercentage = ((float)fence.health / (float)fence.maxHealth) * 100;
    //        GameEventsManager.current.FenceHealthChanged(fence.side, healthPercentage);
    //        yield return new WaitForSeconds(repairSpeed);
    //    }
    //}
}
