using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    PlayerStats playerStats;
    public float maxHealth = 100;
    public float regenRate = 1;

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


    public static event Action<PlayerHealth, float> OnHealthChangedEvent; // New event

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        maxHealth = playerStats.constitution * 25;
        regenRate = playerStats.constitution;
        Health = maxHealth;
    }

    private void Update()
    {
        // Natural Regeneration
        if (Health < maxHealth)
        {
            Heal(regenRate * Time.deltaTime / 60); // Regenerate health over time
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