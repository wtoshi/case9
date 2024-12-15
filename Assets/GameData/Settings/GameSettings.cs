using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Magical Tower/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("General Settings")]
    public int initialRound = 1; 

    [Header("Tower Settings")]
    public List<Tower> towers;

    [Header("Enemy Settings")]
    public List<Enemy> enemies;

    [Header("Spawn Settings")]
    public int baseEnemyCount = 10;
    public float spawnInterval = 1f;
    public float spawnRadius = 10f;
    public float minSpawnDistance = 5f;
    public float enemySpacing = 1.5f;
    public float spawnCountIncreaseRate = 1.1f;
    public float baseEnemyWeight = 1f;
    public float levelWeightMultiplier = 0.5f;

}
