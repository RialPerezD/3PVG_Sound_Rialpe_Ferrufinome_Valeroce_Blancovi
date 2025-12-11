using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// First-Person Character Controller that handles movement, physics-based jumping, 
/// camera rotation, and FMOD-based footstep audio.
/// </summary>
/// <remarks>
/// <para><strong>Requirements:</strong></para>
/// <list type="bullet">
/// <item><description>Must have a <see cref="CharacterController"/> attached.</description></item>
/// <item><description>Requires FMOD for audio playback.</description></item>
/// </list>
/// <para><strong>Features:</strong></para>
/// <list type="bullet">
/// <item><description>Coyote Time for more responsive jumping.</description></item>
/// <item><description>KillZone reset logic.</description></item>
/// </list>
/// </remarks>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Configuration")]
    /// <summary>
    /// Movement speed in units per second.
    /// </summary>
    public float speed = 5f;

    /// <summary>
    /// Mouse sensitivity for camera rotation (Look X and Y).
    /// </summary>
    public float sensitivity = 200f;

    /// <summary>
    /// The maximum height the player can reach during a jump.
    /// </summary>
    public float jumpHeight = 1.5f;

    /// <summary>
    /// Custom gravity force applied to the player (usually negative).
    /// </summary>
    public float gravity = -9.81f;

    [Header("References")]
    /// <summary>
    /// Reference to the Camera transform to apply vertical pitch rotation.
    /// </summary>
    public new Transform camera;

    [Header("FMOD Configuration")]
    /// <summary>
    /// The FMOD Event Reference for the footstep sound.
    /// </summary>
    public EventReference sound;

    /// <summary>
    /// Time in seconds between footstep sounds while moving.
    /// </summary>
    public float interval = 0.5f;

    /// <summary>
    /// Current vertical rotation of the camera (Pitch).
    /// </summary>
    private float _rotationX = 0f;

    /// <summary>
    /// Reference to the Unity CharacterController component.
    /// </summary>
    private CharacterController _controller;

    /// <summary>
    /// Countdown timer for the next footstep sound.
    /// </summary>
    private float timerfootsteps = 0f;

    /// <summary>
    /// Current velocity vector, primarily used for vertical physics (gravity/jumping).
    /// </summary>
    private Vector3 _velocity;

    /// <summary>
    /// Timer for "Coyote Time". Allows the player to jump briefly after walking off a ledge.
    /// </summary>
    private float _groundedTimer;

    /// <summary>
    /// Stores the position where the player started the scene for respawning.
    /// </summary>
    private Vector3 _initialPosition;

    /// <summary>
    /// Initializes references, locks the cursor, and stores the spawn position.
    /// </summary>
    void Start()
    {
        _controller = GetComponent<CharacterController>();

        // Setup cursor for FPS style control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _initialPosition = transform.position;
    }

    /// <summary>
    /// Main update loop handling input, view, movement, and system commands.
    /// </summary>
    private void Update()
    {
        HandleLook();
        HandleMovement();

        // Exit application
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    /// <summary>
    /// Processes mouse input to rotate the camera (vertical) and the player body (horizontal).
    /// </summary>
    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        _rotationX -= mouseY;
        // Clamp vertical look to prevent neck breaking (90 degrees up/down)
        _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

        camera.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Processes movement input, applies gravity, and handles jumping logic.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="_groundedTimer"/> to implement Coyote Time.
    /// </remarks>
    void HandleMovement()
    {
        // Ground check and Coyote Time logic
        if (_controller.isGrounded)
        {
            _groundedTimer = 0.2f; // Reset grace period

            // Keep a small negative velocity to ensure the controller sticks to the ground
            if (_velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }
        else
        {
            _groundedTimer -= Time.deltaTime;
        }

        // Horizontal Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move * (speed * Time.deltaTime));

        // Jump Logic (Physics equation: v = sqrt(h * -2 * g))
        // Allows jumping if the player is grounded OR within the Coyote Time window
        if (Input.GetButtonDown("Jump") && _groundedTimer > 0)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Consume the timer to prevent double jumping immediately
            _groundedTimer = 0;
        }

        // Apply Gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // Handle Audio
        FootSteps(move);
    }

    /// <summary>
    /// Determines when to play footstep sounds based on movement and ground state.
    /// </summary>
    /// <param name="vectorMovimiento">The current movement vector of the player.</param>
    void FootSteps(Vector3 vectorMovimiento)
    {
        // Only play sound if grounded and actually moving
        if (_controller.isGrounded && vectorMovimiento.magnitude > 0.1f)
        {
            timerfootsteps -= Time.deltaTime;

            if (timerfootsteps <= 0)
            {
                PlaySound();
                timerfootsteps = interval;
            }
        }
        else
        {
            // Reset timer so sound plays immediately when starting to walk again
            timerfootsteps = 0;
        }
    }

    /// <summary>
    /// Plays the FMOD OneShot sound at the player's position.
    /// </summary>
    void PlaySound()
    {
        if (!sound.IsNull)
        {
            RuntimeManager.PlayOneShot(sound, transform.position);
        }
    }

    /// <summary>
    /// Handles collision triggers, specifically for resetting the player position.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillZone"))
        {
            // Disable controller temporarily to prevent physics conflicts during teleport
            _controller.enabled = false;
            transform.position = _initialPosition;
            _controller.enabled = true;

            Debug.LogWarning("Player has been reset to the initial position.");
        }
    }
}