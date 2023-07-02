<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
=======
using System.Collections.Generic;
using TMPro;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
{
    [SerializeField]
    private EnemyType enemyType;

    [SerializeField]
    public Animator animator;
    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    public GameObject playerObject;
    [SerializeField]
    public Transform playerTransform;
    [SerializeField]
    public LayerMask whatIsGround, whatIsPlayer;
    [SerializeField]
    public float health;
=======
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
>>>>>>> Stashed changes

    [SerializeField]
    private Vector3 centerPoint;
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;

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

<<<<<<< Updated upstream

    private void Awake()
    {
        playerTransform = playerObject.transform;
        agent = GetComponent<NavMeshAgent>();
=======
   
    private void Awake()
    {
        playerTransform = playerObject.transform;
        enemyrb = GetComponent<Rigidbody>();
        aliveEnemies.Add(this);
    }

    private void Start()
    {
        UpdateHealth();      
>>>>>>> Stashed changes
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(centerPoint, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
<<<<<<< Updated upstream
        }
=======
        }      
>>>>>>> Stashed changes
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
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        animator.SetTrigger("IsWalking");
        Vector3 randomDirection = Random.insideUnitSphere * sightRange;
        randomDirection += centerPoint;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
            agent.SetDestination(walkPoint);
        }
    }

    private void ChasePlayer()
    {
        animator.SetTrigger("IsWalking");
        if (Vector3.Distance(centerPoint, playerTransform.position) <= sightRange)
        {
            Vector3 targetPosition = centerPoint + (playerTransform.position - centerPoint).normalized * sightRange;
            agent.SetDestination(targetPosition);
        }
        else
        {
            agent.SetDestination(centerPoint);
        }
    }

    private void AttackPlayer()
    {
<<<<<<< Updated upstream
        // Make sure the enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(playerTransform);

=======
        enemyrb.velocity = Vector3.zero;
        enemyrb.angularVelocity = Vector3.zero;
        transform.LookAt(playerTransform);

>>>>>>> Stashed changes
        if (!alreadyAttacked)
        {
            EnemyAttackSfx();
            animator.SetTrigger("Attack");
<<<<<<< Updated upstream
            // Attack code here
            GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            // End of attack code

            Destroy(projectileObject, 5f);
=======
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            spawnPosition.y = transform.position.y + pivotOffset;

            GameObject projectileObject = Instantiate(projectile, spawnPosition, Quaternion.identity);
            Rigidbody projectileRb = projectileObject.GetComponent<Rigidbody>();
            projectileRb.AddForce(transform.forward * 3f, ForceMode.Impulse);
            Destroy(projectileObject, 3f);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        health -= damage;
=======
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
>>>>>>> Stashed changes

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
<<<<<<< Updated upstream
    {
        Destroy(gameObject);
=======
    {       
        this.gameObject.SetActive(false);       
>>>>>>> Stashed changes
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint, sightRange);
    }
<<<<<<< Updated upstream
=======

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
>>>>>>> Stashed changes
}
