using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject escMenu;
    [Space]

    [Header("Game Settings")]
    [SerializeField] private float aimSensitivity = 100f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f; 
    [SerializeField] private float strengthOfGravity = 13f;
    [SerializeField] private float groundDistanceCheck = 0.4f;
    [SerializeField] private float jumpHeight = 2f;
    [Space]

    private float xRotation;
    private Vector3 velocity;
    private bool isPlayerGrounded;
    private float speed;
    private bool isPaused;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateEscapeMenu();
        IsPlayerRunning();
        /*IsPlayerCrouching();*/
        UpdatePlayerMovement();
        CheckIfPlayerGrounded();
        UpdatePlayerRotation();
    }

    private void UpdateEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            hud.SetActive(false);
            escMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            hud.SetActive(true);
            escMenu.SetActive(false);
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        
    }
    private void IsPlayerRunning()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }

    /*private void IsPlayerCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerCamera.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        }
        else
        {
            playerCamera.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }*/

    private void UpdatePlayerRotation()
    {
        var mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime;

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

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var newMovement = transform.right * x + transform.forward * z;
        
        controller.Move(newMovement * (speed * Time.deltaTime));

        // This controls jumping

        if (isPlayerGrounded && Input.GetButton("Jump"))
            velocity.y = Mathf.Sqrt(2 * jumpHeight * strengthOfGravity);

        if (!isPlayerGrounded)
        {
            velocity.y -= strengthOfGravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
