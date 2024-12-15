using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class GameManager : Singleton<GameManager>
{
    public GameData gameData;
    public GameSettings gameSettings;

    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] Transform spawnPoint;

    public GameEntries.GAME_STATE GameState => gameState;
    GameEntries.GAME_STATE gameState;

    public TowerController CurrentTower => currentTower;
    TowerController currentTower;

    public List<EnemyController> activeEnemies = new List<EnemyController>();

    #region MONOBEHAVIOUR
    protected override void OnAwake()
    {
        base.OnAwake();

        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerSelected.ToString(), OnTowerSelected);
        EventManager.Subscribe(GameEntries.GAME_EVENTS.EnemyKilled.ToString(), OnEnemyKilled);
        EventManager.Subscribe(GameEntries.GAME_EVENTS.EnemySpawned.ToString(), OnEnemySpawned);
        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerLevelUp.ToString(), OnTowerLevelUp);
        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerGotExp.ToString(), OnTowerGotExp);

        gameState = GameEntries.GAME_STATE.LOADING;
        GetGameData();
    }

    private void Start()
    {
        InitGame();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.EnemyKilled.ToString(), OnEnemyKilled);
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerSelected.ToString(), OnTowerSelected);
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.EnemySpawned.ToString(), OnEnemySpawned);
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerLevelUp.ToString(), OnTowerLevelUp);
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerGotExp.ToString(), OnTowerGotExp);
    }
    #endregion

    #region INITIALIZATION
    private void InitGame()
    {
        if (gameData.selectedTowerId >= 0)
        {
            InitMenu();
        }
        else
        {
            Debug.Log("No selected tower id found in game data.");

            InitTowerSelection();            
        }
    }

    private void InitMenu()
    {
        SpawnTower(gameData.selectedTowerId);
        UIManager.Instance.OpenMenuUI();
        gameState = GameEntries.GAME_STATE.MENU;
    }

    private void InitTowerSelection()
    {
        gameState = GameEntries.GAME_STATE.TOWER_SELECTION;
        UIManager.Instance.OpenTowerSelectionUI();
    }
    #endregion

    #region GAME
    public void StartGame()
    {
        UIManager.Instance.OpenGameUI();
        gameState = GameEntries.GAME_STATE.STARTED;
        StartNewRound();
    }

    private void StartNewRound()
    {
        Debug.Log($"Starting Round {gameData.gameLevel}...");
        EventManager.Trigger(GameEntries.GAME_EVENTS.RoundStarted.ToString(), gameData.gameLevel);
        spawnManager.SpawnEnemies(gameData.gameLevel);
    }

    public void SpawnTower(int towerId)
    {
        Tower selectedTower = gameSettings.towers.Find(t => t.id == towerId);

        if (selectedTower == null)
        {
            Debug.LogError("Invalid tower id: " + towerId);
            return;
        }

        GameObject towerInstance = Instantiate(selectedTower.modelPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        currentTower = towerInstance.GetComponent<TowerController>();
        currentTower.InitializeTower(selectedTower, gameData.towerState);
    }
    #endregion

    #region DATA
    private void GetGameData()
    {
        gameData = DataController.Instance.GameData;
    }

    public void UpdateGameLevel(int gameLevel)
    {
        if (gameData == null)
            gameData = new GameData();

        gameData.gameLevel = gameLevel;

        DataController.Instance.SaveGameData(gameData);
    }

    public int GetTowerLevel()
    {
        return gameData.towerState.level;
    }

    public List<Tower> GetTowers()
    {
        return gameSettings.towers;
    }

    public Tower GetTowerById(int towerId)
    {
        return gameSettings.towers.Find(t => t.id == towerId);
    }

    public void UpdateTowerState(int level, int experience)
    {
        if (gameData.towerState == null)
            gameData.towerState = new TowerState();

        gameData.towerState.level = level;
        gameData.towerState.experience = experience;

        DataController.Instance.SaveGameData(gameData);
    }

    void UpdateTowerLevel(int newLevel)
    {
        if (gameData.towerState == null)
            gameData.towerState = new TowerState();

        gameData.towerState.level = newLevel;
        DataController.Instance.SaveGameData(gameData);
    }

    void UpdateTowerExperience(int newExperience)
    {
        if (gameData.towerState == null)
            gameData.towerState = new TowerState();

        gameData.towerState.experience = newExperience;
        DataController.Instance.SaveGameData(gameData);
    }
    #endregion

    #region EVENTS
    public void OnEnemySpawned(object enemy)
    {
        EnemyController enemyController = (EnemyController)enemy;
        activeEnemies.Add(enemyController);
    }

    private void OnEnemyKilled(object enemy)
    {
        EnemyController enemyController = (EnemyController)enemy;

        EventManager.Trigger(GameEntries.GAME_EVENTS.TowerGotExp.ToString(), enemyController.enemyData.experienceReward);

        activeEnemies.RemoveAll(e => e.spawnedId == enemyController.spawnedId);

        if (activeEnemies.Count == 0)
        {
            Debug.Log("All enemies defeated. Starting new round...");
            gameData.gameLevel++;
            DataController.Instance.SaveGameData(gameData);
            StartNewRound();
        }
    }
    private void OnTowerSelected(object towerId)
    {
        int selectedTowerId = (int)towerId;

        if (selectedTowerId < 0)
        {
            Debug.LogError("Invalid tower id: " + selectedTowerId);
            return;
        }

        gameData.selectedTowerId = selectedTowerId;
        gameData.towerState = new TowerState();

        DataController.Instance.SaveGameData(gameData);

        InitMenu();
    }

    private void OnTowerLevelUp(object newLevel)
    {
        int level = (int)newLevel;
        UpdateTowerLevel(level);
    }

    private void OnTowerGotExp(object addingExp)
    {
        int experience = (int)addingExp;

        var newExp = gameData.towerState.experience + experience;
        UpdateTowerExperience(newExp);
    }
    #endregion

}
