using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    float maxHealth = 100;
    float currentHealth;


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            EventManager.Trigger(GameEntries.GAME_EVENTS.TowerDestroyed.ToString());
        }

        UpdateHealthUI();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"Tower healed. Current Health: {currentHealth}/{maxHealth}");

        UpdateHealthUI();
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
    }

    private void UpdateHealthUI()
    {
        // Update health UI
        EventManager.Trigger(GameEntries.GAME_EVENTS.UpdateTowerHealthUI.ToString(), currentHealth / (float)maxHealth);
    }
}
