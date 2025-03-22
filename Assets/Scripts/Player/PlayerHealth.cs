using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float lowHealth = 20;

    private float _health = 100;
    public float Health
    {
        get { return _health; }
        set
        {
            if (_health != value) // Only trigger event if health actually changes
            {
                _health = Mathf.Clamp(value, 0, maxHealth); // Ensure health stays within valid range
                OnHealthChangedEvent?.Invoke(this, _health);
            }
        }
    }

    public float HealthRegenRate = 1; // Health per minute (in game)

    public static event Action<PlayerHealth, float> OnHealthChangedEvent; // New event

    private void Update()
    {
        // Natural Regeneration
        if (Health < maxHealth)
        {
            Heal((HealthRegenRate * Time.deltaTime) / 60); // Regenerate health over time
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public void Heal(float amount)
    {
        Health += amount;
    }
}