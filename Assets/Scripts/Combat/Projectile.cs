using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    TowerController tower;
    EnemyController target; 

    Ability ability;

    private Vector3 startPosition;

    private Vector3 targetPosition;
    private bool isLaunched = false;

    private bool isReady;
    private bool hasHitTarget = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (target != null && !hasHitTarget)
        {
            MoveTowardsTarget();
        }else if(ability.targetType == GameEntries.TARGET_TYPE.RandomPosition && !hasHitTarget)
        {
            MoveToPosition();
        }
        else
        {
            MoveForward();
        }
    }
    public void Initialize(Ability ability, TowerController tower, EnemyController target = null)
    {
        this.tower = tower;
        this.ability = ability;

        this.target = target;

        transform.localScale = ability.projectilePrefabScale;

        if (ability.targetType == GameEntries.TARGET_TYPE.RandomPosition)
        {
            SetRandomGroundPosition();
        }
        else if (target != null)
        {
            targetPosition = target.transform.position;
        }
        else
        {
            Debug.LogError("No valid target for the projectile.");
            Destroy(gameObject);
        }

        isReady = true;
        Destroy(gameObject, ability.projectileDuration);
    }

    private void SetRandomGroundPosition()
    {
        float randomX = Random.Range(-ability.range, ability.range);
        float randomZ = Random.Range(-ability.range, ability.range);
        targetPosition = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);
    }

    private void MoveTowardsTarget()
    {       
        transform.LookAt(target.transform);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, ability.speed * Time.deltaTime);
    }

    private void MoveToPosition()
    {
        float progress = Vector3.Distance(startPosition, transform.position) / Vector3.Distance(startPosition, targetPosition);
        float heightOffset = Mathf.Sin(progress * Mathf.PI) * 1.0f; 

        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, ability.speed * Time.deltaTime);
        nextPosition.y = Mathf.Lerp(startPosition.y, targetPosition.y, progress) + heightOffset;

        transform.position = nextPosition;
    }

    private void MoveForward()
    {
        transform.position += transform.forward * ability.speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<EnemyController>();

        Debug.Log($"Collider Name: {other.name}");
        if (enemy != null)
        {
            if (enemy.IsDead)
                return;

            HitTarget(enemy);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Projectile hit the ground.");
            EnvironmentHit();
        }
    }

    private void HitTarget(EnemyController hitTarget = null)
    {
        hasHitTarget = true;

        if (hitTarget != null)
        {
            CombatManager.Instance.ExecuteProjectileHit(tower, hitTarget, ability);
        }

        if (ability.hitPrefab != null)
        {
            InitHitPrefab();
        }

        DestroyProjectile();
    }

    private void EnvironmentHit()
    {
        if (ability.hitPrefab != null)
        {            
            InitHitPrefab();
        }

        DestroyProjectile();
    }

    private void InitHitPrefab()
    {
        var hitOjb = Instantiate(ability.hitPrefab, transform.position, Quaternion.identity);
        hitOjb.transform.localScale = ability.hitPrefabScale;

        if (ability.hitClip != null)
        {
            AudioSource audioSource = hitOjb.AddComponent<AudioSource>();
            audioSource.clip = ability.shotClip;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.Play();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
