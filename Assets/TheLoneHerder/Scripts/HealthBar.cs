using UnityEngine;
using UnityEngine.UI;

namespace TheLoneHerder
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] int fenceSide;
        [SerializeField] Image fill;
        Slider slider;
        [SerializeField] Gradient gradient;
        Animator animator;
        [SerializeField] Transform player;
        AudioSource ding;

        private void Start()
        {
            slider = GetComponent<Slider>();
            animator = GetComponent<Animator>();
            ding = GetComponent<AudioSource>();
        }
        private void Update()
        {
            transform.LookAt(player);
        }

        private void OnEnable()
        {
            GameEventsManager.current.onFenceHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            GameEventsManager.current.onFenceHealthChanged -= UpdateHealthBar;
        }

        private void UpdateHealthBar(int side, float healthPercentage)
        {
            if (side == fenceSide)
            {
                slider.value = healthPercentage;
                fill.color = gradient.Evaluate(healthPercentage / 100);
                animator.SetFloat("health", healthPercentage);
                if (healthPercentage == 100)
                {
                    ding.Play();
                    animator.SetTrigger("maxHealthAchieved");
                }
            }
        }
    }
}