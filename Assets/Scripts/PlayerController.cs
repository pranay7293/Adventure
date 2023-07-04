using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text LeveltextMesh;
    [SerializeField]
    private GameObject GoldCup;
    [SerializeField]
    private GameObject HandCup;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Rigidbody playerrb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private  ShootingCtrl shootingCtrl;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private TextMeshProUGUI playerHealth;
    [SerializeField]
    private TextMeshProUGUI EnemiesAlive;

    private float BottomClamp = -10f;
    private float TopClamp = 20f;
    private float mouseX, mouseY;
    private float sensitivity = 2f;
    private float maxHealth = 100;
    private float currentHealth;

    public bool isgamePaused = false;
    public int enemycount;

    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHealth = maxHealth;   
    }

    void Update()
    {
        if (!isgamePaused)
        {
            Movement();            
        }
        CameraRotation();
        EnemiesAliveUI();
        UpdateHealthBar();
    }

    public void CameraRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        mouseX = (mouseX + 360f) % 360f;
        mouseY = Mathf.Clamp(mouseY, BottomClamp, TopClamp);

        mainCamera.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
    }

    private void Movement()
    {
        float vertical = Input.GetAxis("Vertical"); 
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 movement = transform.forward * vertical * speed * Time.deltaTime;
        if(Mathf.Abs(vertical) > 0.2)
        {
            animator.SetBool("PistolAim", false);           
        }
        animator.SetFloat("Speed", Mathf.Abs(vertical));
        playerrb.MovePosition(transform.position + movement);

        if (vertical != 0 || horizontal != 0)
        {
            Vector3 inputDirection = new Vector3(horizontal, 0.0f, vertical).normalized;
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!isgamePaused)
        {
            float playerdamage;
            switch (collision.tag)
            {
                case "BlueProjectile":
                    playerdamage = Random.Range(5, 8 + 1);
                    TakeDamage(playerdamage);
                    break;
                case "YellowProjectile":
                    playerdamage = Random.Range(10, 12 + 1);
                    TakeDamage(playerdamage);
                    break;
                case "MinoProjectile":
                    playerdamage = Random.Range(20, 25 + 1);
                    TakeDamage(playerdamage);
                    break;
                case "LevelComplete":
                    LevelComplete();
                    break;

                default:
                    break;
            }
        }
        
    }
    public void EnemiesAliveUI()
    {
        enemycount = EnemyController.GetAliveEnemyCount();
        EnemiesAlive.text = "Enemies Alive: " + enemycount.ToString();
    }

    public void LevelComplete()
    {
        if (enemycount == 0)
        {
            LeveltextMesh.gameObject.SetActive(true);
            LeveltextMesh.text = "Level Complete!!";
            LeveltextMesh.color = Color.green;
            GoldCup.gameObject.SetActive(false);
            HandCup.gameObject.SetActive(true);
            animator.SetTrigger("Victory");
            SoundManager.Instance.PlaySound(Sounds.PlayerWin);
            isgamePaused = true;
            shootingCtrl.isGame = true;
            Invoke(nameof(YouWin), 5f);
        }
        else
        {
            LeveltextMesh.gameObject.SetActive(true);
            LeveltextMesh.text = "Kill all the enemies!";
            LeveltextMesh.color = Color.red;
            SoundManager.Instance.PlaySound(Sounds.Error);
            animator.SetTrigger("No");
        }
    }    

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetTrigger("Die");
            SoundManager.Instance.PlaySound(Sounds.PlayerDie);            
            isgamePaused = true;
            shootingCtrl.isGame = true;
            Invoke(nameof(GameLost), 5f);
        }
        else
        { if(damageAmount >= 5)
            {                
                SoundManager.Instance.PlaySound(Sounds.PlayerHit);
                animator.SetTrigger("GetHit");
            }    
        }
        UpdateHealthBar();
    }

    private void GameLost()
    {
        gameManager.GameOver();
    }

    private void YouWin()
    {
        gameManager.PlayerWin();
    }

    private void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
        playerHealth.text = currentHealth.ToString("F1"); ;
    }

}