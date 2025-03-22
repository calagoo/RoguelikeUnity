using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthUI : MonoBehaviour
{
    public EnemyHealth enemyHealth; // Reference to PlayerHealth script
    public Slider healthSlider; // Reference to UI Slider

    private void Start()
    {
        if (enemyHealth != null)
        {
            // Initialize slider
            healthSlider.maxValue = enemyHealth.maxHealth;
            healthSlider.value = enemyHealth.Health;
        }

        // Subscribe to health change event
        EnemyHealth.OnHealthChangedEvent += UpdateEnemyHealthUI;
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        EnemyHealth.OnHealthChangedEvent -= UpdateEnemyHealthUI;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateEnemyHealthUI(EnemyHealth enemy, float newHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = newHealth; // Update the UI slider
            if (newHealth <= enemy.lowHealth * enemy.maxHealth)
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
            else if (newHealth == enemy.maxHealth)
            {
                // Disable health bar
                healthSlider.fillRect.GetComponent<Image>().color = Color.clear;
            }
            else
            {
                healthSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
        }
    }

}
