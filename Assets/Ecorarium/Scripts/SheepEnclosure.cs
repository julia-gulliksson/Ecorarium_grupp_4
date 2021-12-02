using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SheepEnclosure : MonoBehaviour
{
    int wolvesAttacking = 0;
    int health = 100;
    bool isHealthTicking = false;
    [SerializeField] TMP_Text healthUI;

    private void OnEnable()
    {
        GameEventsManager.current.onWolfCollide += IncrementWolvesAttacking;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfCollide -= IncrementWolvesAttacking;
    }

    void IncrementWolvesAttacking(bool attacking, int id)
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
                health--;
                healthUI.text = health.ToString();
                yield return new WaitForSeconds(0.7f / wolvesAttacking);
            }
        }
        else
        {
            isHealthTicking = false;
        }
    }
}
