using UnityEngine;

public class TowerCombat : MonoBehaviour
{
    [SerializeField] private TowerController towerController;

    public void TakeDamage(float damage)
    {
        towerController.TowerHealth.TakeDamage(damage);
        EventManager.Trigger(GameEntries.GAME_EVENTS.TowerDamaged.ToString(), damage);
    }

    public void Heal(float healAmount)
    {
        towerController.TowerHealth.Heal(healAmount);
        EventManager.Trigger(GameEntries.GAME_EVENTS.TowerHealed.ToString(), healAmount);
        Debug.Log($"Tower healed for {healAmount}");
    }
}
