using System.Collections;
using UnityEngine;

public class FenceRepairableState : FenceBaseState
{
    FenceStateManager fence;
    float repairSpeed = 0.1f;
    float resetSpeed = 0.05f;
    bool isRepairing = false;
    bool isResetting = false;
    int savedHealth;
    bool maxHealthAchieved;

    public override void EnterState(FenceStateManager fenceRef)
    {
        fence = fenceRef;
        savedHealth = fence.Health;
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(0) && !isRepairing && !maxHealthAchieved)
        {
            fence.repairHealth = fence.StartCoroutine(Repair());
            if (isResetting) fence.StopCoroutine(fence.resetHealth);

        }
        else if (!Input.GetMouseButton(0) && isRepairing)
        {
            if (!isResetting && !maxHealthAchieved) fence.resetHealth = fence.StartCoroutine(ResetHealth());
            fence.StopCoroutine(fence.repairHealth);
        }
    }

    public override void ExitState()
    {
        if (isRepairing && !maxHealthAchieved)
        {
            fence.StopCoroutine(fence.repairHealth);
            fence.resetHealth = fence.StartCoroutine(ResetHealth());
        }
    }

    IEnumerator Repair()
    {
        isRepairing = true;
        isResetting = false;
        while (savedHealth < fence.MaxHealth)
        {
            savedHealth++;
            if (fence.side == 1)
            {
                Debug.Log("Repairing! " + savedHealth);

            }
            fence.SendUpdatedHealth(savedHealth);
            fence.UpdateHealth(savedHealth);
            yield return new WaitForSeconds(repairSpeed);
        }
        if (savedHealth >= fence.MaxHealth) maxHealthAchieved = true;
    }

    IEnumerator ResetHealth()
    {
        isResetting = true;
        isRepairing = false;
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
    }
}
