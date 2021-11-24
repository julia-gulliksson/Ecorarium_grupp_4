using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepEnclosure : MonoBehaviour
{
    int wolvesAttacking = 0;
    int health = 100;
    bool isHealthTicking = false;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfCollide += IncrementWolvesAttacking;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfCollide -= IncrementWolvesAttacking;
    }

    void IncrementWolvesAttacking(bool attacking)
    {
        if (attacking)
        {
            wolvesAttacking++;
        }
        else
        {
            wolvesAttacking--;
        }
        if (!isHealthTicking)
        {
            StartCoroutine(DoDamage());
        }
    }

    IEnumerator DoDamage()
    {
        isHealthTicking = true;
        if (wolvesAttacking > 0)
        {
            while (wolvesAttacking > 0 && health > 0)
            {
                Debug.Log("Wolves attacking");
                health--;
                Debug.Log(health + " HEALTH");
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            isHealthTicking = false;
        }
    }
}
