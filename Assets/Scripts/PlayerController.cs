using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    private float BottomClamp = -15f;
    private float TopClamp = 30f;

    public Animator animator;
    public float speed = 3f;
    private float mouseX, mouseY;
    private float sensitivity = 1f;
    [SerializeField]
    private Rigidbody playerrb;

    private void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
    }
   

    void Update()
    {
        Movement();
        CameraRotation();
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

}