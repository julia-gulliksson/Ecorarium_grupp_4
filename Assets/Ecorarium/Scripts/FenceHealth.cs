using System.Collections;
using UnityEngine;

public class FenceHealth : MonoBehaviour
{
    int wolvesAttacking = 0;
    int baseHealth = 100;
    int health;
    int maxHealth;
    bool isHealthTicking = false;
    float tickSpeed = 0.1f;
    float repairSpeed = 0.1f;
    float resetSpeed = 0.05f;
    [SerializeField] public int side;
    bool isRepairing = false;
    bool isResetting = false;
    int damagedHealth;
    private Coroutine repair;
    private Coroutine reset;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfFoundTarget += IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget += DecrementWolvesAttacking;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfFoundTarget -= IncrementWolvesAttacking;
        GameEventsManager.current.onWolfLostTarget -= DecrementWolvesAttacking;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !isRepairing)
        {
            if (side == 1) Debug.Log("Start repair");
            repair = StartCoroutine(Repair());
            if (isResetting) StopCoroutine(reset);

        }
        if (!Input.GetMouseButton(0) && isRepairing)
        {
            if (side == 1) Debug.Log("STOP REPAIR");
            StopCoroutine(repair);
            if (!isResetting)
            {
                //TODO: Only reset if health is not maxhealth
                reset = StartCoroutine(ResetHealth());
            }
        }
    }

    private void Start()
    {
        int children = 0;
        foreach (Transform childTransform in transform)
        {
            if (childTransform.name.Contains("Fence")) children++;
        }
        // Base health on amount of fence assets this side has
        maxHealth = baseHealth * children;
        health = maxHealth;
    }

    void IncrementWolvesAttacking(int fenceSide)
    {
        if (fenceSide == side)
        {
            wolvesAttacking++;

            if (!isHealthTicking)
            {
                StartCoroutine(DoDamage());
            }
        }
    }

    void DecrementWolvesAttacking(int fenceSide)
    {
        if (fenceSide == side)
        {
            wolvesAttacking--;
            if (wolvesAttacking <= 0)
            {
                StopCoroutine(DoDamage());
                isHealthTicking = false;
                damagedHealth = health;
            }
        }
    }

    IEnumerator DoDamage()
    {
        isHealthTicking = true;
        if (wolvesAttacking > 0)
        {
            while (wolvesAttacking > 0 && health > 0)
            {
                health--;
                float healthPercentage = ((float)health / (float)maxHealth) * 100;
                GameEventsManager.current.FenceHealthChanged(side, healthPercentage);
                yield return new WaitForSeconds(tickSpeed / wolvesAttacking);
            }
        }
        else
        {
            isHealthTicking = false;
        }
        if (health <= 0)
        {
            GameEventsManager.current.FenceBroke();
            Destroy(gameObject);
        }
    }

    IEnumerator ResetHealth()
    {
        isResetting = true;
        isRepairing = false;
        while (health > damagedHealth)
        {
            health--;
            if (side == 1)
            {

                Debug.Log("Resetting! " + health);
            }
            float healthPercentage = ((float)health / (float)maxHealth) * 100;
            GameEventsManager.current.FenceHealthChanged(side, healthPercentage);
            yield return new WaitForSeconds(resetSpeed);
        }
    }

    IEnumerator Repair()
    {
        isRepairing = true;
        isResetting = false;
        while (health < maxHealth)
        {
            health++;
            if (side == 1)
            {
                Debug.Log("Repairing! " + health);

            }
            float healthPercentage = ((float)health / (float)maxHealth) * 100;
            GameEventsManager.current.FenceHealthChanged(side, healthPercentage);
            yield return new WaitForSeconds(repairSpeed);
        }
    }
}
