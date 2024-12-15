using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAbility", menuName = "Magical Tower/Abilities/New Ability")]
public class Ability : ScriptableObject
{
    [Header("General Info")]
    public int id;
    public string abilityName;
    public string displayName;
    public int minLevel;

    [Header("Settings")]
    public GameEntries.ABILITY_MECHANIC_TYPE mechanicType;
    public GameEntries.TARGET_TYPE targetType;

    [Header("Visuals")]
    public GameObject projectilePrefab;
    public Vector3 projectilePrefabScale;
    public bool useSocket;
    public GameEntries.SOCKET_TYPE projectileSocketType; 
    public Vector3 projectilePositionOffset;
    public bool useHitSocket;
    public GameEntries.SOCKET_TYPE hitSocketType;
    public Vector3 hitPositionOffset;
    public GameObject hitPrefab;
    public Vector3 hitPrefabScale;
    public Sprite abilityIcon;

    [Header("Effects Applied")]
    public List<Effect> effects;

    [Header("Sound Settings")]
    public AudioClip shotClip;
    public AudioClip hitClip;
    public bool attachSoundToProjectile;

    [Header("Mechanics")]
    public bool useGravity;
    public float range; 
    public float projectileDuration;
    public bool explosive;
    public float explosionRadius; 
    public float speed; 
    public float cooldown; 
}