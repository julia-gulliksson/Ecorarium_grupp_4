using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinyRock : MonoBehaviour
{
    public AudioSource Sparkly;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Stimk")
        {
            Sparkly.Play();
        }
    }
}
