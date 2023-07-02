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

    public Camera mainCamera;
    private float BottomClamp = -15f;
    private float TopClamp = 30f;
    public Animator animator;
    public float speed = 3f;
    private float mouseX, mouseY;
    private float sensitivity = 2f;

    private CharacterController characterController;

    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBarSlider;
    public EnemyController enemyController;
    public GameManager gameManager;
    [SerializeField]
    private Rigidbody playerrb;

    public bool isgamePaused = false;
    public int enemycount;


    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (!isgamePaused)
        {
            Movement();            
        }
        CameraRotation();
        enemycount = EnemyController.GetAliveEnemyCount();
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
        Debug.Log("Movement called");
        float vertical = Input.GetAxis("Vertical"); // Use "VerticalWASD" input axis for forward/backward movement
        float horizontal = Input.GetAxis("Horizontal"); // Use "HorizontalWASD" input axis for left/right movement

        Vector3 movement = transform.forward * vertical * speed * Time.deltaTime;
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
        int playerdamage;
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
            Invoke(nameof(YouWin), 3f);
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

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount >= currentHealth)
        {
            currentHealth = 0;
            SoundManager.Instance.PlaySound(Sounds.PlayerDie);
            animator.SetTrigger("Die");            
            Invoke(nameof(GameLost), 3f);
        }
        else
        {
            currentHealth -= damageAmount;
            SoundManager.Instance.PlaySound(Sounds.PlayerHit);
            animator.SetTrigger("GetHit");
            UpdateHealthBar();
        }
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
    }

}