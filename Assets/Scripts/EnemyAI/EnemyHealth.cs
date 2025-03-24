using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float _health = 100;
    public float maxHealth = 100;
    // low health is 25% of max health
    public float lowHealth = 0.25f;
    public float regenRate = 1; // Health per minute

    public NPCAI npcAI;

    public float Health
    {
        get { return _health; }
        set
        {
            if (_health != value) // Only trigger event if health actually changes
            {
                _health = Mathf.Clamp(value, -maxHealth, maxHealth); // Ensure health stays within valid range
                OnHealthChangedEvent?.Invoke(this, _health);
            }
        }
    }
    public static event Action<EnemyHealth, float> OnHealthChangedEvent; // New event

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Health = Mathf.Clamp(Health, -maxHealth, maxHealth);

        // If health goes below 0, disable AI, leave ragdoll
        // If health goes below -maxHealth, destroy Object
        //
        // This will allow the "garbage mobs" to deal damage to the ragdoll,
        // until it is destroyed.
        //
        // Also if you do do much damage it will evaporate the enemy.
        if (Health <= -maxHealth)
            Destroy(gameObject);
        else if (Health <= 0)
            npcAI.enabled = false;
        else
            Heal(regenRate * Time.deltaTime / 60);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void Heal(float amount)
    {
        Health += amount;
    }
}
