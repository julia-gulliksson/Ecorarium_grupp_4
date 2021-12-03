using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FenceHealth : MonoBehaviour
{
    int wolvesAttacking = 0;
    int health = 100;
    bool isHealthTicking = false;

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

    void IncrementWolvesAttacking()
    {
        wolvesAttacking++;

        if (!isHealthTicking)
        {
            StartCoroutine(DoDamage());
        }
    }

    void DecrementWolvesAttacking()
    {
        wolvesAttacking--;
    }

    IEnumerator DoDamage()
    {
        isHealthTicking = true;
        if (wolvesAttacking > 0)
        {
            while (wolvesAttacking > 0 && health > 0)
            {
                health--;
                Debug.Log(health);
                //healthUI.text = health.ToString();
                yield return new WaitForSeconds(2f / wolvesAttacking);
            }
        }
        else
        {
            isHealthTicking = false;
        }
    }
}
