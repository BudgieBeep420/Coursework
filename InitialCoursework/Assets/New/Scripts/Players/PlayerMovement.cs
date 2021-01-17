using System.IO;
using Actual_Game_Files.Scripts;
using UnityEngine;

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

    [SerializeField] public float userDefinedSens = 1f;

    private void Start()
    {
        /*Sets the cursor as invisible, and locks it to the center as you would expect,
            then grabs the game settings so we can use the user-defined sensitivity*/
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var gameSettingsDirectory = Directory.GetCurrentDirectory() + @"\Settings\GameSettings.json";
        var gameSettingsProfile = JsonUtility.FromJson<GameSettingsProfile>(File.ReadAllText(gameSettingsDirectory));
        userDefinedSens = gameSettingsProfile.sensitivity;
    }

    /* This is called on every frame */
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

    /* This is used instead of RigidBody.velocity */
    private void UpdateActualVelocity()
    {
        _vel = Vector3.Distance(_lastPosition, transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
    }
    
    /* Checks if the player is pressing escape key, if so, brings up the escape menu or HUD*/
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
        /* This gets the current inputs from the mouse in the frame, and multiplies them by the sensitivity */
        var mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime * userDefinedSens;
        var mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime * userDefinedSens;

        /* This sets the rotation of the mouse upwards (inverted as mouse up should = look up) */
        _xRotation -= mouseY;
        
        /* This clamps the values, so you can't flip the camera over */
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        /* This applies the rotation */
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
        /*Finds the values input into the keyboard */
        var x = Input.GetAxis("Horizontal"); 
        var z = Input.GetAxis("Vertical");

        var t = transform;

        /* This calculates the movement being done in this frame */
        var newMovement = t.right * x + t.forward * z;
        
        /* This invokes the character controller, and moves the players transform */
        controller.Move(newMovement * (_speed * Time.deltaTime));


        /* This checks if the player is grounded and jumps if they press space in this frame
            (See CheckIfPlayerGrounded();)*/
        if (_isPlayerGrounded && Input.GetButton("Jump"))
            _velocity.y = Mathf.Sqrt(2 * jumpHeight * strengthOfGravity);
        
        /* This adds a downwards velocity each second (acceleration) down to the floor if the player is in the air */
        if (!_isPlayerGrounded)
        {
            _velocity.y -= strengthOfGravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }

        /* This does the final movement */
        controller.Move(_velocity * Time.deltaTime);
    }

    private void UpdatePlayerSound()
    {
        /* This plays if the player is on the floor and walking */
        if (_isPlayerGrounded && _vel > 2 && !feetAudioSource.isPlaying || Input.GetKeyUp(KeyCode.LeftShift))
            audioManager.Play("WalkingSound", feetAudioSource);
        
        /* This plays if the player is on the floor and running */
        if (_isPlayerGrounded && Input.GetKey(KeyCode.LeftShift) && _vel > 3  && (feetAudioSource.clip.name == "walking_loop" || !feetAudioSource.isPlaying))
            audioManager.Play("RunningSound", feetAudioSource);
        
        /* This stops the current movement sound if the player is in the air */
        if (!_isPlayerGrounded || _vel < 2)
            feetAudioSource.Stop();
    }
}
