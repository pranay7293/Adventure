using System.Collections;
using System.Collections.Generic;
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

    //States
    public bool playerInSightRange = false;
    public bool playerInAttackRange = false;


    private void Awake()
    {
        playerTransform = playerObject.transform;
        agent = GetComponent<NavMeshAgent>();
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
        }
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
        // Make sure the enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(playerTransform);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            // Attack code here
            GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            // End of attack code

            Destroy(projectileObject, 5f);
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
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPoint, sightRange);
    }
}
