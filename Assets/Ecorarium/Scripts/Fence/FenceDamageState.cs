using System.Collections;
using UnityEngine;

public class FenceDamageState : FenceBaseState
{
    FenceStateManager fence;
    int wolvesAttacking = 0;
    bool isHealthTicking = false;
    float tickSpeed = 0.1f;

    public override void EnterState(FenceStateManager fenceRef)
    {
        Debug.Log("Hello from damage state");
        fence = fenceRef;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        fence.StopCoroutine(DoDamage());
    }

    public void IncrementWolvesAttacking(int fenceSide)
    {
        if (fenceSide == fence.side)
        {
            wolvesAttacking++;

            if (!isHealthTicking)
            {
                fence.StartCoroutine(DoDamage());
            }
        }
    }

    public void DecrementWolvesAttacking(int fenceSide)
    {
        if (fenceSide == fence.side)
        {
            wolvesAttacking--;
            if (wolvesAttacking <= 0)
            {
                fence.StopCoroutine(DoDamage());
                isHealthTicking = false;
                fence.damagedHealth = fence.health;
            }
        }
    }

    IEnumerator DoDamage()
    {
        isHealthTicking = true;
        if (wolvesAttacking > 0)
        {
            while (wolvesAttacking > 0 && fence.health > 0)
            {
                fence.health--;
                float healthPercentage = ((float)fence.health / (float)fence.maxHealth) * 100;
                GameEventsManager.current.FenceHealthChanged(fence.side, healthPercentage);
                yield return new WaitForSeconds(tickSpeed / wolvesAttacking);
            }
        }
        else
        {
            isHealthTicking = false;
        }
        if (fence.health <= 0)
        {
            GameEventsManager.current.FenceBroke();
            Object.Destroy(fence.gameObject);
        }
    }
}
