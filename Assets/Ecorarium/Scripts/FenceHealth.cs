using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FenceHealth : MonoBehaviour
{
    int wolvesAttacking = 0;
    int baseHealth = 100;
    int health;
    int maxHealth;
    bool isHealthTicking = false;
    float tickSpeed = 2f;
    [SerializeField] public int side;

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
            //TODO: Signal to wolves to advance, game over
            Destroy(gameObject);
        }
    }
}
