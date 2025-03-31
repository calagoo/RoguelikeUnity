using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUIScript : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to PlayerHealth script
    public Slider healthSlider; // Reference to UI Slider

    private void Start()
    {
        if (playerHealth != null)
        {
            // Initialize slider
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.Health;
        }

        // Subscribe to health change event
        // PlayerHealth.OnHealthChangedEvent += UpdateHealthUI;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        // PlayerHealth.OnHealthChangedEvent -= UpdateHealthUI;
    }

    private void UpdateHealthUI(PlayerHealth player, float newHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = newHealth; // Update the UI slider
            if (newHealth <= 0.25f * player.maxHealth)
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
            else
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
        }
    }
}