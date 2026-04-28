using UnityEngine;
using UnityEngine.InputSystem;

public class FalconController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 6f;
    public float boostSpeed = 16f;

    [Header("Movement Smoothing")]
    public float rotationSpeed = 10f;

    [Header("Shoulder Anchor")]
    public Transform shoulderAnchor;
    public float anchorFollowSpeed = 12f;
    public float anchorRotationSpeed = 12f;

    private Vector2 moveInput;
    private float verticalInput;

    private bool isActiveCharacter = false;
    private bool isBoosting = false;

    private void Update()
    {
        if (isActiveCharacter)
        {
            HandleMovement();
        }
        else
        {
            AnchorToShoulder();
        }
    }

    private void HandleMovement()
    {
        Vector3 horizontalMove = new Vector3(moveInput.x, 0f, moveInput.y);
        horizontalMove = Vector3.ClampMagnitude(horizontalMove, 1f);

        Vector3 verticalMove = Vector3.up * verticalInput;

        Vector3 finalMoveDirection = horizontalMove + verticalMove;
        finalMoveDirection = Vector3.ClampMagnitude(finalMoveDirection, 1f);

        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;

        transform.position += finalMoveDirection * currentSpeed * Time.deltaTime;

        if (horizontalMove.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMove);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void AnchorToShoulder()
    {
        if (shoulderAnchor == null)
            return;

        transform.position = Vector3.Lerp(
            transform.position,
            shoulderAnchor.position,
            anchorFollowSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            shoulderAnchor.rotation,
            anchorRotationSpeed * Time.deltaTime
        );
    }

    public void SetActiveCharacter(bool active)
    {
        isActiveCharacter = active;

        if (!active)
        {
            moveInput = Vector2.zero;
            verticalInput = 0f;
            isBoosting = false;
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

    public void OnFlyVertical(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        verticalInput = context.ReadValue<float>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        if (context.performed)
        {
            isBoosting = true;
        }

        if (context.canceled)
        {
            isBoosting = false;
        }
    }
}