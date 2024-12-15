using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "Magical Tower/Tower/New Tower")]
public class Tower : ScriptableObject
{
    public int id;
    public string towerName; 
    public string towerDescription;
    public Sprite towerIcon;
    public GameObject modelPrefab; 
    public int maxHealth;
    public float damageModifier; 
    public float rangeModifier;
    public float attackSpeedModifier;

    public List<Ability> Abilities;
}
