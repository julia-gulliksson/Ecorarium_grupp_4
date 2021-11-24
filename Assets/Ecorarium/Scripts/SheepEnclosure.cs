using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepEnclosure : MonoBehaviour
{

    GameObject positioning;
    int wolvesAttacking = 0;
    int health;

    private void Start()
    {
        positioning = new GameObject("Positioning");
        //StartCoroutine(CreateTransformPoints());
    }

    private void OnEnable()
    {
        GameEventsManager.current.onWolfCollide += DetectWolves;
    }

    private void OnDisable()
    {
        GameEventsManager.current.onWolfCollide -= DetectWolves;
    }

    void DetectWolves(bool found)
    {
        if (found)
        {
            wolvesAttacking++;
        }
        else
        {
            wolvesAttacking--;
        }

        Debug.Log(wolvesAttacking + " collisions");
    }

    IEnumerator CreateTransformPoints()
    {
        Debug.Log(transform.position + " POSITION");
        Debug.Log(transform.localScale + " SCALE");
        float centerX = transform.localScale.x / 2;
        float plusX = transform.position.x + centerX;
        float minusX = transform.position.x - centerX;
        Debug.Log(transform.position.x + " POSITION X");
        Debug.Log(centerX + "CENTER X");
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(plusX, minusX);
            Debug.Log(x + " XXX");
            Instantiate(positioning, new Vector3(x, transform.position.y, transform.position.z), Quaternion.identity);
            yield return null;
        }
    }
}
