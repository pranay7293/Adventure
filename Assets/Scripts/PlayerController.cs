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


    public bool isgamePaused = false;
    public int enemycount;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
<<<<<<< Updated upstream

    void Update()
    {
        Movement();
        Rotation();
=======
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
>>>>>>> Stashed changes
    }

    private void Rotation()
    {
         mouseX += Input.GetAxis("Mouse X") * sensitivity;
         mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        transform.eulerAngles = new Vector3(0f, mouseX, 0f);
        mainCamera.transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
    }


    private void Movement()
    {
<<<<<<< Updated upstream
        Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
        move = this.transform.TransformDirection(move) * speed;

        characterController.Move(move * Time.deltaTime);

        float movement = move.magnitude;
        Debug.Log("value" + movement);
        animator.SetFloat("Speed", Mathf.Abs(movement));
    }

    
=======
        float vertical = Input.GetAxis("Vertical"); // Use "VerticalWASD" input axis for forward/backward movement
        float horizontal = Input.GetAxis("Horizontal"); // Use "HorizontalWASD" input axis for left/right movement
        if (Mathf.Abs(vertical) > 0)
        {
            animator.SetBool("PistolAim", false);
            animator.SetFloat("Speed", Mathf.Abs(vertical)); 
        }
        Vector3 movement = transform.forward * vertical * speed * Time.deltaTime;
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

>>>>>>> Stashed changes
}