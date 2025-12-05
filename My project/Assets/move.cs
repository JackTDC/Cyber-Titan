using UnityEngine;
using PinePie.SimpleJoystick; // ✅ Correct namespace for PinePie joystick

[RequireComponent(typeof(CharacterController))]
public class move : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Animation Settings")]
    public Animator animator;
    private int speedHash = Animator.StringToHash("Speed");

    [Header("References")]
    public JoystickController joystick; // ✅ PinePie joystick class
    private CharacterController controller;
    private Camera mainCam;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Auto-find PinePie joystick if not assigned
        if (joystick == null)
        {
            JoystickController[] joysticks = FindObjectsOfType<JoystickController>();
            foreach (var j in joysticks)
            {
                if (j.name.Contains("Joystick"))
                {
                    joystick = j;
                    break;
                }
            }
        }

        if (joystick == null)
            Debug.LogError("No PinePie JoystickController found! Please assign one in the inspector.");
    }

    void Update()
    {
        if (joystick == null) return;

        // --- Joystick Input ---
        Vector2 input = joystick.InputDirection; // (x = horizontal, y = vertical)
        Vector3 move = new Vector3(input.x, 0, input.y);
        float inputMagnitude = Mathf.Clamp01(move.magnitude);

        // --- Camera-relative movement ---
        if (mainCam != null)
        {
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            move = camForward * input.y + camRight * input.x;
        }

        // --- Rotate toward movement direction ---
        if (inputMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- Move the character ---
        controller.Move(move * moveSpeed * inputMagnitude * Time.deltaTime);

        // --- Gravity ---
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // --- Animator speed blend ---
        if (animator != null)
            animator.SetFloat(speedHash, inputMagnitude, 0.1f, Time.deltaTime);
    }
}
