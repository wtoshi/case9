using System.Collections.Generic;
using UnityEngine;

public class CombatButtonController : MonoBehaviour
{
    [SerializeField] GameObject combatButtonPF;

    List<CombatButton> combatButtons = new List<CombatButton>();

    TowerController tower;

    void Start()
    {
        tower = GameManager.Instance.CurrentTower;

        InitCombatButtons();
    }

    void InitCombatButtons()
    {
        foreach (var ability in tower.towerData.Abilities)
        {
            GameObject combatButtonGO = Instantiate(combatButtonPF, transform);
            CombatButton combatButton = combatButtonGO.GetComponent<CombatButton>();
            combatButton.Set(this, tower, ability);

            combatButtons.Add(combatButton);
        }

    }
}
