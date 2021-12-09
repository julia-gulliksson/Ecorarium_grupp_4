using System.Collections;
using UnityEngine;

namespace TheLoneHerder
{
    public class FenceResetState : FenceBaseState
    {
        FenceStateManager fence;
        float resetSpeed = 0.05f;
        int savedHealth;
        bool isResetting = false;

        public override void EnterState(FenceStateManager fenceRef)
        {
            fence = fenceRef;
            // Save a reference to the current fence health
            savedHealth = fence.Health;

            if (!fence.MaxHealthReached() && savedHealth > fence.DamagedHealth)
            {
                fence.resetHealth = fence.StartCoroutine(ResetHealth());
            }
            else
            {
                fence.SwitchState(fence.DamageState);
            }
        }

        public override void ExitState()
        {
            if (isResetting)
            {
                fence.StopCoroutine(fence.resetHealth);
                isResetting = false;
            }
        }

        IEnumerator ResetHealth()
        {
            isResetting = true;
            while (savedHealth > fence.DamagedHealth)
            {
                savedHealth--;
                if (fence.side == 1)
                {

                    Debug.Log("Resetting! " + savedHealth);
                }
                fence.SendUpdatedHealth(savedHealth);
                fence.UpdateHealth(savedHealth);
                yield return new WaitForSeconds(resetSpeed);
            }

            // Health is back to the damaged health, return to DamageState
            fence.SwitchState(fence.DamageState);
        }
    }
}