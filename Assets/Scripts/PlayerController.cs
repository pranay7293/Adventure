using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0f, mouseX, 0f);

        float forwardMovement = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * speed * forwardMovement * Time.deltaTime;

        // Move the character controller
        characterController.Move(movement);

        // Set the animation speed based on the absolute value of the forward movement input
        animator.SetFloat("Speed", Mathf.Abs(forwardMovement));

    }


}
