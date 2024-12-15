using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int spawnedId;

    [SerializeField] private Rigidbody rb;

    [HideInInspector]
    public Enemy enemyData; 

    private Transform tower;
    private float currentHealth;
    private float attackCooldown;

    public bool IsDead => isDead;
    bool isDead = false;

    private void Update()
    {
        if (tower != null && enemyData != null)
        {
            MoveTowardsTower();
            CheckAttackTower();
        }
    }

    public void InitializeEnemy(Enemy enemyData, int spawnId)
    {
        this.spawnedId = spawnId;

        this.enemyData = enemyData;
        currentHealth = enemyData.health;
        attackCooldown = 0f;

        GameObject towerObject = GameObject.FindWithTag("Tower");
        if (towerObject != null)
        {
            tower = towerObject.transform;
        }
        else
        {
            Debug.LogError("Tower not found in the scene.");
        }
    }

    private void MoveTowardsTower()
    {
        float distanceToTower = Vector3.Distance(transform.position, tower.position);

        if (distanceToTower > enemyData.attackDistance)
        {
            // Move
            transform.position = Vector3.MoveTowards(
                transform.position,
                tower.position,
                enemyData.speed / 5 * Time.deltaTime
            );

            // Look At
            Vector3 directionToTower = (tower.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTower.x, 0, directionToTower.z)); 
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void CheckAttackTower()
    {
        if (Vector3.Distance(transform.position, tower.position) <= enemyData.attackDistance)
        {
            if (attackCooldown <= 0f)
            {
                AttackTower();
                attackCooldown = 10f / enemyData.attackSpeed; 
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    private void AttackTower()
    {
        Debug.Log($"{enemyData.enemyName} is attacking the tower with {enemyData.attackPower} damage!");

        TowerController towerController = tower.GetComponent<TowerController>();
        if (towerController != null)
        {
            towerController.TowerCombat.TakeDamage(enemyData.attackPower);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody found on enemy for knockback.");
            return;
        }

        Vector3 knockback = direction.normalized * force;
        rb.AddForce(knockback, ForceMode.Impulse);
        Debug.Log($"{name} knocked back with force: {force}");
    }

    private void Die()
    {
        isDead = true;
        EventManager.Trigger(GameEntries.GAME_EVENTS.EnemyKilled.ToString(), this);
        Destroy(gameObject);
    }
}
