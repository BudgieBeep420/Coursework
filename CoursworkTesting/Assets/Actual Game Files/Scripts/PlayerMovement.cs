using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;



    [SerializeField] private float aimSensitivity = 100f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float StrengthOfGravity = 13f;
    [SerializeField] private float groundDistanceCheck = 0.4f;
    [SerializeField] private float jumpHeight = 2f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isPlayerGrounded;
    private float speed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        IsPlayerRunning();
        IsPlayerCrouching();
        UpdatePlayerMovement();
        CheckIfPlayerGrounded();
        UpdatePlayerRotation();
    }

    private void IsPlayerRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    private void IsPlayerCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerCamera.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }
        else
        {
            playerCamera.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }

    private void UpdatePlayerRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void CheckIfPlayerGrounded()
    {
        isPlayerGrounded = Physics.CheckSphere(groundChecker.position, groundDistanceCheck, groundMask);

        // This resets the y velocity if it is on the floor
        if (isPlayerGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void UpdatePlayerMovement()
    {
        // General WASD movement 

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 newMovement = transform.right * x + transform.forward * z;
        controller.Move(newMovement * speed * Time.deltaTime);

        // This controls jumping

        if (isPlayerGrounded == true && Input.GetButton("Jump"))
        {
            Debug.Log(velocity.y);
            velocity.y = Mathf.Sqrt(2 * jumpHeight * StrengthOfGravity);
        }

        // This part controls the gravity

        if (!isPlayerGrounded)
        {
            velocity.y -= StrengthOfGravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }



        controller.Move(velocity * Time.deltaTime);
    }
}
