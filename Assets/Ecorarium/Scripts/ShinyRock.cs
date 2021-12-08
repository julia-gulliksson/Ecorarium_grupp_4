using UnityEngine;

public class ShinyRock : MonoBehaviour
{
    public AudioSource Sparkly;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Weapon"))
        {
            Sparkly.Play();
        }
    }
}
