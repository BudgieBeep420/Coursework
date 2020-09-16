using System.Collections;
using System.Collections.Generic;
using Actual_Game_Files.Scripts;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource feetAudioSource;
    [Space]

    [Header("Game Settings")]
    [SerializeField] private float aimSensitivity = 100f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f; 
    [SerializeField] private float strengthOfGravity = 13f;
    [SerializeField] private float groundDistanceCheck = 0.4f;
    [SerializeField] private float jumpHeight = 1f;
    [Space]

    private float _xRotation;
    private Vector3 _velocity;
    private bool _isPlayerGrounded;
    private float _speed;
    public bool isPaused;
    private Vector3 _lastPosition;
    private float _vel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        UpdateEscapeMenu();
        IsPlayerRunning();
        /*IsPlayerCrouching();*/
        UpdatePlayerMovement();
        CheckIfPlayerGrounded();
        UpdatePlayerRotation();
        UpdateActualVelocity();
        UpdatePlayerSound();
    }

    private void UpdateActualVelocity()
    {
        _vel = Vector3.Distance(_lastPosition, transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
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
            ReturnToGame();
    }

    public void ReturnToGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        hud.SetActive(true);
        escMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    private void IsPlayerRunning()
    {
        _speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
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

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void CheckIfPlayerGrounded()
    {
        _isPlayerGrounded = Physics.CheckSphere(groundChecker.position, groundDistanceCheck, groundMask);

        // This resets the y velocity if it is on the floor
        if (_isPlayerGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    private void UpdatePlayerMovement()
    { 
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var newMovement = transform.right * x + transform.forward * z;
        
        controller.Move(newMovement * (_speed * Time.deltaTime));


        if (_isPlayerGrounded && Input.GetButton("Jump"))
            _velocity.y = Mathf.Sqrt(2 * jumpHeight * strengthOfGravity);

        if (!_isPlayerGrounded)
        {
            _velocity.y -= strengthOfGravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }

        controller.Move(_velocity * Time.deltaTime);
    }

    private void UpdatePlayerSound()
    {
        if (_isPlayerGrounded && _vel > 2 && !feetAudioSource.isPlaying || Input.GetKeyUp(KeyCode.LeftShift))
            audioManager.Play("WalkingSound", feetAudioSource);
        
        if (_isPlayerGrounded && Input.GetKey(KeyCode.LeftShift) && _vel > 3  && (feetAudioSource.clip.name == "walking_loop" || !feetAudioSource.isPlaying))
            audioManager.Play("RunningSound", feetAudioSource);
        
        if (!_isPlayerGrounded || _vel < 2)
            feetAudioSource.Stop();
    }
}
