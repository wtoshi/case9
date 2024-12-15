using UnityEngine;

public class TowerController : MonoBehaviour
{
    [HideInInspector]
    public Tower towerData;
    GameData.TowerState towerState;

    public TowerHealth TowerHealth { get { return towerHealth; } }
    public TowerLevel TowerLevel { get { return towerLevel; } }
    public TowerCombat TowerCombat { get { return towerCombat; } }
    public TowerSlotController TowerSlotController { get { return towerSlotController; } }

    [SerializeField] TowerHealth towerHealth;
    [SerializeField] TowerLevel towerLevel;
    [SerializeField] TowerCombat towerCombat;
    [SerializeField] TowerSlotController towerSlotController;

    #region MONOBEHAVIOUR
    private void Awake()
    {
        Assigns();
        SubscribeEvents();
    }

    private void Start()
    {
        SetComponents();
    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
    #endregion

    #region INITALIZATION
    void Assigns()
    {
        towerHealth = GetComponent<TowerHealth>();
        towerLevel = GetComponent<TowerLevel>();
    }

    void SetComponents()
    {
        towerLevel.Set(towerState.level, towerState.experience);
    }

    public void InitializeTower(Tower towerData, GameData.TowerState towerState)
    {
        this.towerData = towerData;
        this.towerState = towerState;

        towerHealth.SetMaxHealth(towerData.maxHealth);

        Debug.Log($"Initialized Tower: {towerData.towerName}");
    }
    #endregion

    #region COMBAT
    public void InitAbility(Ability ability, EnemyController target)
    {
        CombatManager.Instance.InitAbility(ability,this, target);
    }



    #endregion

    #region EVENTS

    void SubscribeEvents()
    {
        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerDestroyed.ToString(), OnTowerDestroyed);
    }

    void UnSubscribeEvents()
    {
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerDestroyed.ToString(), OnTowerDestroyed);
    }

    private void OnTowerDestroyed(object data)
    {
        Debug.Log("Tower Destroyed!");
    }
    #endregion

}
