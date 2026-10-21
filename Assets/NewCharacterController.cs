
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewCharacterController : MonoBehaviour
{
    [Header("Movimiento")]
    public float WalkSpeed = 3f;
    public float SprintSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float rotationSpeed = 10f;
    public float mouseSensivity = 1f;
    public float gravity = -20f;

    [Header("Referenciación")]
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 Velocity;
    private float currentSpeed;
    private float yaw;
    private Vector3 externalVelocity = Vector3.zero;

    public bool IsMoving { get; private set; }
    public Vector2 CurrentInput { get; private set; }
    public bool IsGrounded { get; private set; }
    public float CurrentYaw => yaw;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        yaw = transform.eulerAngles.y;
        currentSpeed = WalkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        IsGrounded = characterController.isGrounded;

        if (IsGrounded && Velocity.y < 0f)
        {
            Velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        CurrentInput = new Vector2(horizontal, vertical);

        Vector3 inputDirection =
            new Vector3(horizontal, 0f, vertical).normalized;

        IsMoving = inputDirection.sqrMagnitude > 0.01f;

        currentSpeed = Input.GetKey(KeyCode.LeftShift)
            ? SprintSpeed
            : WalkSpeed;

        Vector3 moveDirection = inputDirection;

        if (cameraTransform != null)
        {
            moveDirection =
                Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f)
                * inputDirection;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Velocity.y += gravity * Time.deltaTime;

        Vector3 horizontalMovement =
            (moveDirection * currentSpeed + externalVelocity)
            * Time.deltaTime;

        Vector3 verticalMovement =
            Vector3.up * Velocity.y * Time.deltaTime;

        characterController.Move(horizontalMovement + verticalMovement);
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity;
        yaw += mouseX;

        if (IsMoving)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}

