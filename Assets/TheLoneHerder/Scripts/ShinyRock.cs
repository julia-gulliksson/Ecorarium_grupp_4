using UnityEngine;

namespace TheLoneHerder
{
    public class ShinyRock : MonoBehaviour
    {
        public AudioSource Sparkly;
        [SerializeField]
        ParticleSystem sparkle;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.name == "Stick")
            {
                Sparkly.Play();
            }
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.name == "Stick")
            {
                Sparkly.Stop();
                sparkle.Stop();
            }
        }
    }
}