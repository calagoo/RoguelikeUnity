using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMana : MonoBehaviour
{
    public float maxMana;
    private float _mana;
    public float ManaRegenRate = 1; // Mana per minute (in game)
    public event Action<PlayerMana, float> OnManaChangedEvent;

    public float Mana
    {
        get { return _mana; }
        set
        {
            if (_mana != value)
            {
                _mana = Mathf.Clamp(value, 0, maxMana);
                OnManaChangedEvent?.Invoke(this, _mana);
            }
        }
    }

    void Start()
    {
        maxMana = PlayerStats.Instance.intelligence;
        Mana = maxMana; // Start full
    }

    private void Update()
    {
        // Natural Regeneration
        if (Mana < maxMana)
        {
            Heal(ManaRegenRate * Time.deltaTime / 60); // Regenerate mana over time
        }
    }

    public void TakeDamage(float amount)
    {
        Mana -= amount;
    }

    public void Heal(float amount)
    {
        Mana += amount;
    }
}