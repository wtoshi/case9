using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform enemyHolder;

    GameSettings gameSettings;

    private int currentRound;
    private int spawnCounter = 0;

    private void Start()
    {
        gameSettings = GameManager.Instance.gameSettings;
    }

    public void SpawnEnemies(int round)
    {
        currentRound = round;
        StartCoroutine(DoSpawnEnemies());
    }

    private IEnumerator DoSpawnEnemies()
    {
        int totalEnemiesToSpawn = Mathf.RoundToInt(CalculateEnemyCount(currentRound));

        for (int i = 0; i < totalEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPoint();

            if (spawnPosition != Vector3.zero) 
            {
                SpawnEnemy(spawnPosition);
            }
            else
            {
                Debug.LogWarning("Could not find a valid spawn position!");
            }

            yield return new WaitForSeconds(gameSettings.spawnInterval);
        }
    }

    private void SpawnEnemy(Vector3 position)
    {
        Enemy selectedEnemy = SelectEnemyBasedOnWeight();

        GameObject enemyInstance = Instantiate(selectedEnemy.modelPrefab, position, Quaternion.identity, enemyHolder);

        EnemyController controller = enemyInstance.GetComponent<EnemyController>();

        controller.InitializeEnemy(selectedEnemy, ++spawnCounter);        

        EventManager.Trigger(GameEntries.GAME_EVENTS.EnemySpawned.ToString(), controller);

        Debug.Log($"Spawned {selectedEnemy.enemyName} at {position} with spawn ID: {controller.spawnedId}");
    }

    private Vector3 GetValidSpawnPoint()
    {
        var towerPos = GameManager.Instance.CurrentTower.transform.position; 
        for (int attempt = 0; attempt < 10; attempt++) 
        {
            Vector2 randomPoint = Random.insideUnitCircle.normalized * Random.Range(gameSettings.minSpawnDistance, gameSettings.spawnRadius);
            Vector3 spawnPoint = new Vector3(towerPos.x + randomPoint.x, 0 , towerPos.z + randomPoint.y);

            if (!IsPointColliding(spawnPoint))
            {
                return spawnPoint; 
            }
        }

        return Vector3.zero; 
    }

    private bool IsPointColliding(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, gameSettings.enemySpacing);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) 
            {
                return true;
            }
        }

        return false;
    }

    private float CalculateEnemyCount(int round)
    {
        return gameSettings.baseEnemyCount * Mathf.Pow(gameSettings.spawnCountIncreaseRate, round - 1);
    }

    private Enemy SelectEnemyBasedOnWeight()
    {
        float totalWeight = 0f;

        foreach (var enemy in gameSettings.enemies)
        {
            float weight = gameSettings.baseEnemyWeight;

            weight += enemy.enemyLevel * gameSettings.levelWeightMultiplier;

            totalWeight += weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in gameSettings.enemies)
        {
            float weight = gameSettings.baseEnemyWeight;


            weight += enemy.enemyLevel * gameSettings.levelWeightMultiplier;
            cumulativeWeight += weight;

            if (randomValue <= cumulativeWeight)
            {
                return enemy;
            }
        }

        return gameSettings.enemies[0];
    }
}
