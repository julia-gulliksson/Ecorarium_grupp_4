using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] int fenceSide;
    [SerializeField] Image fill;
    Slider slider;
    [SerializeField] Gradient gradient;

    private void Start()
    {
        slider = GetComponent<Slider>();
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
        }
    }
}
