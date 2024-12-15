using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    public void InitAbility(Ability ability, TowerController tower, EnemyController enemy = null)
    {
        Debug.Log($"Initialized Ability: {ability.abilityName}");

        switch (ability.mechanicType)
        {
            case GameEntries.ABILITY_MECHANIC_TYPE.Self:
                ExecuteSelfAbility(ability, tower);
                break;
            case GameEntries.ABILITY_MECHANIC_TYPE.Target_Instant:
                ExecuteTargetInstantAbility(ability, tower, enemy);
                break;
            case GameEntries.ABILITY_MECHANIC_TYPE.Projectile:
                ExecuteProjectileAbility(ability, tower, enemy);
                break;
            case GameEntries.ABILITY_MECHANIC_TYPE.Target_Projectile:
                ExecuteTargetProjectileAbility(ability, tower, enemy);
                break;
            case GameEntries.ABILITY_MECHANIC_TYPE.AreaOfEffect:
                ExecuteAreaOfEffectAbility(ability, tower);
                break;
        }
    }

    void ExecuteSelfAbility(Ability ability, TowerController tower)
    {
        Debug.Log($"Executing Self Ability: {ability.abilityName}");
        ExecuteAbilityEffects(ability, tower);
    }

    void ExecuteTargetInstantAbility(Ability ability, TowerController tower, EnemyController target = null)
    {
        Debug.Log($"Executing Target Instant Ability: {ability.abilityName}");


        if (target != null)
        {
            ExecuteAbilityEffects(ability, tower, target);

            return;
        }

        if (target == null)
        {
            var targets = getEnemies(tower.transform.position, ability, ability.targetType);

            if (targets != null && targets.Count > 0)
            {
                foreach (var t in targets)
                {
                    ExecuteAbilityEffects(ability, tower, target);
                }
            }

            return;
        }

        Debug.LogWarning("No valid target found for Target Instant Ability.");
    }

    void ExecuteProjectileAbility(Ability ability, TowerController tower, EnemyController target = null)
    {
        Debug.Log($"Executing Projectile Ability: {ability.abilityName}");

        if (ability.projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab is not assigned.");
            return;
        }

        var spawnPosition = tower.transform.position;

        if(ability.useSocket)
            spawnPosition = tower.TowerSlotController.GetSocketPosition(ability.projectileSocketType);

        GameObject projectile = Instantiate(ability.projectilePrefab, spawnPosition, Quaternion.identity);
        initProjectileComponents(projectile, ability);

        Projectile projectileComponent = projectile.AddComponent<Projectile>();
        projectileComponent.Initialize(ability, tower, target);
    }

    void ExecuteTargetProjectileAbility(Ability ability, TowerController tower, EnemyController target = null)
    {
        Debug.Log($"Executing Target Projectile Ability: {ability.abilityName}");

        if (target == null)
        {
            var targets = getEnemies(tower.transform.position, ability, ability.targetType);

            foreach (var t in targets)
            {
                ExecuteProjectileAbility(ability, tower, t);
            }

            return;
        }

        if (target != null)
        {
            ExecuteProjectileAbility(ability, tower, target);
        }
        else
        {
            Debug.LogWarning("No valid target found for Target Projectile Ability.");
        }
    }

    void ExecuteAreaOfEffectAbility(Ability ability, TowerController tower)
    {
        Debug.Log($"Executing Area of Effect Ability: {ability.abilityName}");

        Collider[] colliders = Physics.OverlapSphere(tower.transform.position, ability.range);
        foreach (var collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                ExecuteAbilityEffects(ability, tower, enemy);
            }
        }
    }

    public void ExecuteProjectileHit(TowerController tower, EnemyController hitTarget, Ability ability)
    {
        if (ability.explosive)
        {
            Collider[] colliders = Physics.OverlapSphere(hitTarget.transform.position, ability.explosionRadius);
            foreach (var collider in colliders)
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    ExecuteAbilityEffects(ability, tower, enemy);
                }
            }
        }
        else
        {
            ExecuteAbilityEffects(ability, tower, hitTarget);
        }
    }

    void ExecuteAbilityEffects(Ability ability, TowerController tower = null, EnemyController target = null)
    {
        Debug.Log($"Executing Ability Effects: {ability.abilityName}");

        foreach (var effect in ability.effects)
        {
            Debug.Log($"Applying Effect: {effect.name}");

            switch (effect.effectType)
            {
                case GameEntries.EFFECT_TYPE.InstantDamage:
                    int damage = Random.Range(effect.MinDamage, effect.MaxDamage);
                    target?.TakeDamage(damage);
                    Debug.Log($"{target.name} took {damage} damage.");
                    break;

                case GameEntries.EFFECT_TYPE.InstantHeal:
                    int healAmount = Random.Range(effect.MinDamage, effect.MaxDamage);
                    tower?.TowerCombat.Heal(healAmount);
                    Debug.Log($"{tower.name} healed for {healAmount}.");
                    break;

                case GameEntries.EFFECT_TYPE.Knockback:
                    if (target != null)
                    {
                        Vector3 knockbackDirection = (target.transform.position - tower.transform.position).normalized;
                        target.ApplyKnockback(knockbackDirection, effect.MaxDamage); 
                        Debug.Log($"{target.name} was knocked back.");
                    }
                    break;
            }
        }
    }

    List<EnemyController> getEnemies(Vector3 position, Ability ability, GameEntries.TARGET_TYPE targetType)
    {
        Collider[] colliders = Physics.OverlapSphere(position, ability.range);
        List<EnemyController> enemies = new List<EnemyController>();

        foreach (var collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            return new List<EnemyController>();
        }

        switch (targetType)
        {
            case GameEntries.TARGET_TYPE.RandomEnemy:
                return new List<EnemyController> { enemies[Random.Range(0, enemies.Count)] };

            case GameEntries.TARGET_TYPE.NearestEnemy:
                enemies.Sort((a, b) =>
                    Vector3.Distance(position, a.transform.position).CompareTo(Vector3.Distance(position, b.transform.position))
                );
                return new List<EnemyController> { enemies[0] };

            case GameEntries.TARGET_TYPE.AllEnemies:
                return enemies;

            default:
                Debug.LogWarning("Invalid target type specified.");
                return new List<EnemyController>();
        }
    }

    private void initProjectileComponents(GameObject projGO, Ability ability)
    {
        Rigidbody rb = projGO.AddComponent<Rigidbody>();
        rb.useGravity = ability.useGravity;
        rb.isKinematic = false;

        SphereCollider sphere = projGO.AddComponent<SphereCollider>();
        sphere.radius = 0.1f;
        sphere.isTrigger = true;

        if (ability.shotClip != null)
        {
            AudioSource audioSource = projGO.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = ability.shotClip;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.Play();
        }
    }

}
