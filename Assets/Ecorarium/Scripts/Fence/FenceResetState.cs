using System.Collections;
using UnityEngine;

public class FenceResetState : FenceBaseState
{
    FenceStateManager fence;
    float resetSpeed = 0.05f;

    public override void EnterState(FenceStateManager fenceRef)
    {
        //fence = fenceRef;
        //Debug.Log("Hello from reset state");
        //fence.StartCoroutine(ResetHealth());
    }

    public override void UpdateState()
    {
        //if (!Input.GetMouseButton(0))
        //{
        //    fence.SwitchState(fence.RepairState);
        //}
    }

    public override void ExitState()
    {
        //fence.StopCoroutine(ResetHealth());
    }

    //IEnumerator ResetHealth()
    //{
    //    while (fence.health > fence.damagedHealth)
    //    {
    //        fence.health--;
    //        if (fence.side == 1)
    //        {

    //            Debug.Log("Resetting! " + fence.health);
    //        }
    //        float healthPercentage = ((float)fence.health / (float)fence.maxHealth) * 100;
    //        GameEventsManager.current.FenceHealthChanged(fence.side, healthPercentage);
    //        yield return new WaitForSeconds(resetSpeed);
    //    }
    //}
}
