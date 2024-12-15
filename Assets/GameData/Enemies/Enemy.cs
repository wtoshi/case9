using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Magical Tower/Enemy/New Enemy")]
public class Enemy : ScriptableObject
{
    public int id; 
    public string enemyName; 
    [TextArea] public string description; 

    [Header("Stats")]
    public int enemyLevel;
    public float health; 
    public float speed; 
    public float attackDistance; 
    public float attackPower; 
    public float attackSpeed; 

    [Header("Visuals")]
    public GameObject modelPrefab;
    public Sprite icon;

    [Header("Rewards")]
    public int experienceReward; 
    public int goldReward; 
}
