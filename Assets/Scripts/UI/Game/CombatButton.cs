using System.Collections;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class CombatButton : MonoBehaviour
{
    public Image abilityIcon;
    public Image cooldownFill;
    public Text cooldownTxt;
    public CanvasGroup thisCG;
    public GameObject reqLevel;

    TowerController tower;
    Ability thisAbility;

    bool isOnCooldown;
    bool isUsable;

    private void Start()
    {
        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerLevelUp.ToString(), OnTowerLevelUp);
    }

    public void Set(CombatButtonController combatButtonController, TowerController tower, Ability ability)
    {
        isUsable = false;
        isOnCooldown = false;

        this.tower = tower;
        thisAbility = ability;
        abilityIcon.sprite = ability.abilityIcon;
        cooldownFill.fillAmount = 0;
        cooldownTxt.text = "";
        thisCG.alpha = 1;

        int towerLevel = GameManager.Instance.GetTowerLevel();

        CheckAndPrepareCombatButton(towerLevel);
    }

    void OnTowerLevelUp(object level)
    {
        int towerLevel = (int)level;
        CheckAndPrepareCombatButton(towerLevel);
    }

    public void CheckAndPrepareCombatButton(int towerLevel)
    {
        if (isUsable)
            return;

        if (towerLevel < thisAbility.minLevel)
        {
            reqLevel.SetActive(true);
            thisCG.interactable = false;
            isUsable = false;
            return;
        }

        reqLevel.SetActive(false);
        isUsable = true;

        SetCooldown(thisAbility.cooldown);
    }

    public void SetCooldown(float cooldown)
    {
        if (cooldown > 0)
        {
            StartCoroutine(CooldownRoutine(cooldown));
        }
    }

    private IEnumerator CooldownRoutine(float cooldown)
    {
        isOnCooldown = true;

        cooldownFill.fillAmount = 1f;
        cooldownTxt.text = cooldown.ToString("F1"); 
        thisCG.alpha = 0.5f; 
        thisCG.interactable = false; 

        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            float remainingTime = cooldown - elapsed;

            cooldownFill.fillAmount = remainingTime / cooldown; 
            cooldownTxt.text = remainingTime.ToString("F1"); 

            yield return null; 
        }

        cooldownFill.fillAmount = 0f;
        cooldownTxt.text = "";
        thisCG.alpha = 1f; 
        thisCG.interactable = true;

        isOnCooldown = false;
    }

    public void OnClick()
    {
        Debug.Log("Clicked on " + thisAbility.displayName);
        this.tower.InitAbility(thisAbility, null);
        SetCooldown(thisAbility.cooldown);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerLevelUp.ToString(), OnTowerLevelUp);
    }
}
