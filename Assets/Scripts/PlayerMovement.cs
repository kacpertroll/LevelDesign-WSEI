using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Movement Settings
    [Header("Movement Settings")] // Ustawienia ruchu
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    [Header("Jump")]
    public bool jumpAllowed = false;
    public float jumpHeight = 2f;
    #endregion
    [Space]
    #region Crouch
    [Header("Crouch Settings")] // Ustawienia kucania
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchTransitionSpeed = 5f;
    #endregion
    [Space]

    // Wa¿ne zmienne!!!
    private CharacterController controller;
    [HideInInspector]
    public Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        HandleMovement();
        HandleCrouch();
        HandleGravity();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = walkSpeed; // Aktualna prêdkoœæ ruchu (do sterowania Head Bobbingiem)

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            currentSpeed = sprintSpeed;
        if (isCrouching)
            currentSpeed = crouchSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && jumpAllowed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            isCrouching = true;

        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            isCrouching = false;
        }

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
    }

    // Metody zwracaj¹ce boola
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}