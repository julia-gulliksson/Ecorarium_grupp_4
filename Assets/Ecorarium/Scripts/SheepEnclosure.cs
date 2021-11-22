using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepEnclosure : MonoBehaviour
{

    private int wolfCollisions;

    private void OnCollisionEnter(Collision collision)
    {
        wolfCollisions++;
        Debug.Log(wolfCollisions);
        if (wolfCollisions >= 10)
        {
            Debug.Log("Complete");
        }
    }
}
