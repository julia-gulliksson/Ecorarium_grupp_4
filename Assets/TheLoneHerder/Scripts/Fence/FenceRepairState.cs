using System.Collections;
using UnityEngine;

namespace TheLoneHerder
{
    public class FenceRepairState : FenceBaseState
    {
        FenceStateManager fence;
        float repairSpeed = 0.02f;
        int savedHealth;
        bool isRepairing = false;

        public override void EnterState(FenceStateManager fenceRef)
        {
            fence = fenceRef;
            savedHealth = fence.Health;
            if (!fence.MaxHealthReached() && !isRepairing)
            {
                fence.repairHealth = fence.StartCoroutine(Repair());
            }
        }

        public override void ExitState()
        {
            if (isRepairing)
            {
                fence.StopCoroutine(fence.repairHealth);
                isRepairing = false;
            }
        }

        IEnumerator Repair()
        {
            isRepairing = true;
            fence.sawingSound.Play();
            while (savedHealth < fence.MaxHealth)
            {
                savedHealth++;
                fence.SendUpdatedHealth(savedHealth);
                fence.UpdateHealth(savedHealth);
                yield return new WaitForSeconds(repairSpeed);
            }
            fence.sawingSound.Stop();

            fence.SwitchState(fence.DamageState);
        }
    }
}