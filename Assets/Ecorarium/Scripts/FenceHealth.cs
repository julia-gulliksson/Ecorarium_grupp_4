using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FenceHealth : MonoBehaviour
{
    int wolvesAttacking = 0;
    int health;
    int baseHealth = 100;
    bool isHealthTicking = false;
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
        // Base health on amount of fence assets this side has
        health = baseHealth * transform.childCount;
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
                Debug.Log(wolvesAttacking + " NR OF WOLVES");
                Debug.Log(side);
                health--;
                Debug.Log("HEALTH: " + health);
                //healthUI.text = health.ToString();
                yield return new WaitForSeconds(2f / wolvesAttacking);
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
