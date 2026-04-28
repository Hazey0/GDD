using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class HumanController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Dash")]
    public float dashSpeed = 14f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isActiveCharacter = true;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravityAlways();

        if (!isActiveCharacter)
        {
            ApplyGravityOnly();
            return;
        }

        HandleCooldowns();
        HandleMovement();
    }

    private void HandleGravityAlways()
    {
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    private void ApplyGravityOnly()
    {
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCooldowns()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDirection.magnitude > 0.1f)
        {
            transform.forward = moveDirection;
        }

        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetActiveCharacter(bool active)
    {
        isActiveCharacter = active;

        if (!active)
        {
            moveInput = Vector2.zero;
            isDashing = false;
        }
    }

    public bool IsActiveCharacter()
    {
        return isActiveCharacter;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        if (!context.performed)
            return;

        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        if (!context.performed)
            return;

        if (dashCooldownTimer > 0f)
            return;

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveDirection.magnitude < 0.1f)
        {
            moveDirection = transform.forward;
        }

        dashDirection = moveDirection.normalized;
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
    }
}