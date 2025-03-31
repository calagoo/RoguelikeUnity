using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UIElements;

public class HudController : MonoBehaviour
{
    public VisualElement ui;
    public ProgressBar healthBar;
    public ProgressBar manaBar;
    public VisualElement healthFillBar; // Reference to the health fill bar
    public VisualElement manaFillBar; // Reference to the mana fill bar
    public PlayerHealth playerHealth; // Reference to PlayerHealth script
    public PlayerMana playerMana; // Reference to PlayerMana script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        healthBar = ui.Q<ProgressBar>("HealthBar");
        manaBar = ui.Q<ProgressBar>("ManaBar");
    }
    void Start()
    {
        // Set the initial values for the health and mana bars
        if (playerHealth != null)
        {
            healthBar.value = playerHealth.Health;
            healthBar.highValue = playerHealth.maxHealth;
            healthFillBar = healthBar.Q(className: "unity-progress-bar__progress");
            healthFillBar.style.backgroundColor = Color.green;
        }
        if (playerMana != null)
        {
            manaBar.value = playerMana.Mana;
            manaBar.highValue = playerMana.maxMana;
            manaFillBar = manaBar.Q(className: "unity-progress-bar__progress");
            manaFillBar.style.backgroundColor = Color.blue;
        }

        // Subscribe to health and mana change events
        if (playerHealth != null)
        {
            playerHealth.OnHealthChangedEvent += UpdateHealthUI;
        }
        if (playerMana != null)
        {
            playerMana.OnManaChangedEvent += UpdateManaUI;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the events to prevent memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnHealthChangedEvent -= UpdateHealthUI;
        }
        if (playerMana != null)
        {
            playerMana.OnManaChangedEvent -= UpdateManaUI;
        }
    }

    private void UpdateHealthUI(PlayerHealth player, float newHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = newHealth; // Update the UI slider
            healthBar.highValue = player.maxHealth; // Update the max value of the slider
            if (newHealth <= 0.25f * player.maxHealth)
            {
                healthFillBar.style.backgroundColor = Color.red; // Change color to red when health is low
            }
            else
            {
                healthFillBar.style.backgroundColor = Color.green; // Change color back to green when health is sufficient
            }
        }
    }

    private void UpdateManaUI(PlayerMana player, float newMana)
    {
        if (manaBar != null)
        {
            manaBar.value = newMana; // Update the UI slider
            manaBar.highValue = player.maxMana; // Update the max value of the slider
            manaFillBar.style.backgroundColor = Color.blue; // Change color to blue for mana
        }
    }
}
