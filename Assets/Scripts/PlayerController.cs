using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public Animator animator;
    public float speed = 3f;
    private float mouseX, mouseY;
    private float sensitivity = 2f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        Rotation();
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
        Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
        move = this.transform.TransformDirection(move) * speed;

        characterController.Move(move * Time.deltaTime);

        float movement = move.magnitude;
        Debug.Log("value" + movement);
        animator.SetFloat("Speed", Mathf.Abs(movement));
    }

    
}