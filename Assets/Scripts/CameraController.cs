using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 2f;
    private Transform playerTransform;

    private float mouseX, mouseY;

    private void Start()
    {
        // Calculate the initial offset between the camera and the player
        playerTransform = transform.parent;
    }

    private void Update()
    {
        // Get the mouse input values
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the player object based on the mouse input
        playerTransform.eulerAngles = new Vector3(0f, mouseX, 0f);

        // Rotate the camera based on the mouse input
        transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
    }
}
