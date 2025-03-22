using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaUIScript : MonoBehaviour
{
    public PlayerMana playerMana; // Reference to PlayerMana script
    public Slider manaSlider; // Reference to UI Slider

    private void Start()
    {
        if (playerMana != null)
        {
            // Initialize slider
            manaSlider.maxValue = playerMana.maxMana;
            manaSlider.value = playerMana.Mana;
        }

        // Subscribe to mana change event
        playerMana.OnManaChangedEvent += UpdateManaUI;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        playerMana.OnManaChangedEvent -= UpdateManaUI;
    }

    private void UpdateManaUI(PlayerMana player, float newMana)
    {
        if (manaSlider != null)
        {
            manaSlider.value = newMana; // Update the UI slider
        }
    }
}