
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    BlackSpider,
    SandSpider,
    Turtle,
    Slime,
    Minotaur    
}
public class EnemyController : MonoBehaviour
{
     
    public EnemyType enemyType;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody enemyrb;    
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private TMP_Text textMeshPro;
    [SerializeField]
    private float health = 100f;   
    [SerializeField]
    private Gradient healthGradient;
    [SerializeField]
    private float pivotOffset;
    [SerializeField]
    private float spawnDistance;

    [SerializeField]
    private Vector3 centerPoint;
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float speed;

    //Patroling
    private Vector3 walkPoint;
    bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    private static List<EnemyController> aliveEnemies = new List<EnemyController>();

    //States
    public bool playerInSightRange = false;
    public bool playerInAttackRange = false;


   
    private void Awake()
    {
        playerTransform = playerObject.transform;
        enemyrb = GetComponent<Rigidbody>();
        aliveEnemies.Add(this);
    }

    private void Start()
    {
        UpdateHealth();      
    }

    private void Update()
    {
        playerInSightRange = Vector3.Distance(centerPoint, playerTransform.position) <= sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, playerTransform.position) <= attackRange;

        if (!playerInSightRange && !playerInAttackRange)
        {
            animator.SetTrigger("IsWalking");
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            animator.SetTrigger("IsWalking");
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void OnDisable()
    {
        aliveEnemies.Remove(this);
    }
    public static int GetAliveEnemyCount()
    {
        return aliveEnemies.Count;
    }
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            if (Vector3.Distance(transform.position, walkPoint) <= 1f)
            {
                walkPointSet = false;
            }
            else
            {
                Vector3 direction1 = (walkPoint - transform.position).normalized;
                enemyrb.MovePosition(transform.position + direction1 * speed);

                if (direction1 != Vector3.zero)
                {
                    Quaternion targetRotation1 = Quaternion.LookRotation(direction1, Vector3.up);
                    enemyrb.MoveRotation(targetRotation1);
                }

            }
        }
    }

    private void SearchWalkPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * sightRange;
        walkPoint = centerPoint + new Vector3(randomPoint.x, 0f, randomPoint.y);
        walkPointSet = true;
    }


    private void ChasePlayer()
    {
        if (Vector3.Distance(centerPoint, playerTransform.position) <= sightRange)
        {
            Vector3 direction2 = (playerTransform.position - transform.position).normalized;
            enemyrb.MovePosition(transform.position + direction2 * speed);

            if (direction2 != Vector3.zero)
            {
                Quaternion targetRotation2 = Quaternion.LookRotation(direction2, Vector3.up);
                enemyrb.MoveRotation(targetRotation2);
            }
        }
        else
        {
            Vector3 direction3 = (centerPoint - transform.position).normalized;
            enemyrb.MovePosition(transform.position + direction3 * speed);
            if (direction3 != Vector3.zero)
            {
                Quaternion targetRotation3 = Quaternion.LookRotation(direction3, Vector3.up);
                enemyrb.MoveRotation(targetRotation3);
            }
        }
    }

    private void AttackPlayer()
    {
        // Make sure the enemy doesn't move
        enemyrb.velocity = Vector3.zero;
        enemyrb.angularVelocity = Vector3.zero;
        transform.LookAt(playerTransform);


        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            // Attack code here
            GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody projectileRb = projectileObject.GetComponent<Rigidbody>();
            projectileRb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            projectileRb.AddForce(transform.up * 8f, ForceMode.Impulse);
            // End of attack code

            Destroy(projectileObject, 3f);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {

        if (damage >= health)
        {
            health = 0;
            UpdateHealth();
            EnemyDeathSfx();
            animator.SetTrigger("Die");
            Invoke(nameof(DestroyEnemy), 3f);
        }
        else
        {
            health -= damage;
            UpdateHealth();
            EnemyHitSfx();
            animator.SetTrigger("GetHit");
        }
    }
    
    private void UpdateHealth()
    {
        float healthPercentage = health / 100f;
        Color currentColor = healthGradient.Evaluate(healthPercentage);
        textMeshPro.text = health.ToString("F0");
        textMeshPro.color = currentColor;
    }      
    
    private void DestroyEnemy()

    {       
        this.gameObject.SetActive(false);       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint, sightRange);
    }

    private void EnemyAttackSfx()
    {
        switch (enemyType)
        {
            case EnemyType.BlackSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderAttack);
                break;
            case EnemyType.SandSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderAttack);
                break;
            case EnemyType.Turtle:
                SoundManager.Instance.PlaySound(Sounds.GoblinAttack);
                break;
            case EnemyType.Slime:
                SoundManager.Instance.PlaySound(Sounds.GoblinAttack);
                break;
            case EnemyType.Minotaur:
                SoundManager.Instance.PlaySound(Sounds.MinoAttack);
                break;
            default:
                break;
        }
    }

    private void EnemyDeathSfx()
    {
        switch (enemyType)
        {
            case EnemyType.BlackSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderDie);
                break;
            case EnemyType.SandSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderDie);
                break;
            case EnemyType.Turtle:
                SoundManager.Instance.PlaySound(Sounds.GoblinDie);
                break;
            case EnemyType.Slime:
                SoundManager.Instance.PlaySound(Sounds.GoblinDie);
                break;
            case EnemyType.Minotaur:
                SoundManager.Instance.PlaySound(Sounds.MinoDie);
                break;
            default:
                break;
        }
    }
    private void EnemyHitSfx()
    {
        switch (enemyType)
        {
            case EnemyType.BlackSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderHit);
                break;
            case EnemyType.SandSpider:
                SoundManager.Instance.PlaySound(Sounds.SpiderHit);
                break;
            case EnemyType.Turtle:
                SoundManager.Instance.PlaySound(Sounds.GoblinHit);
                break;
            case EnemyType.Slime:
                SoundManager.Instance.PlaySound(Sounds.GoblinHit);
                break;
            case EnemyType.Minotaur:
                SoundManager.Instance.PlaySound(Sounds.MinoHit);
                break;
            default:
                break;
        }
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

}
