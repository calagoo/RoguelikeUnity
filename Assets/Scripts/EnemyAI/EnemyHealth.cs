using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float _health = 100;
    public float maxHealth = 100;
    // low health is 25% of max health
    public float lowHealth = 0.25f;
    public float regenRate = 1;

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
        Health += regenRate * Time.deltaTime/60;
        Health = Mathf.Clamp(Health, 0, maxHealth);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}
