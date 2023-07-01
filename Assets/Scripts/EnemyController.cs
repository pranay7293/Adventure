using TMPro;
using UnityEngine;

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
    private Vector3 centerPoint;
    [SerializeField]
    private float sightRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float speed = 0.1f;

    //Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;    
    public GameObject projectile;

    //States
    private bool alreadyAttacked = false;
    private bool playerInSightRange = false;
    private bool playerInAttackRange = false;

    private void Start()
    {
        UpdateHealth();
    }

    private void Awake()
    {
        playerTransform = playerObject.transform;
        enemyrb = GetComponent<Rigidbody>();
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
            animator.SetTrigger("Die");
            Invoke(nameof(DestroyEnemy), 3f);
        }
        else
        {
            health -= damage;
            UpdateHealth();
            animator.SetTrigger("GetHit");
        }
    }
    
    private void UpdateHealth()
    {
        float healthPercentage = health / 100f;
        Color currentColor = healthGradient.Evaluate(healthPercentage);
        Debug.Log("healthpersentage" + healthPercentage);
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

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}
