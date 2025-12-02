using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI_Slider : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.maxValue = playerHealth.MaxHealth;
        slider.value = playerHealth.CurrentHealth;
    }

    private void Update()
    {
        slider.value = playerHealth.CurrentHealth;
    }
}
