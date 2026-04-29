using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
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

    private Rigidbody rb;

    private Vector2 moveInput;
    private float verticalInput;

    private bool isActiveCharacter = false;
    private bool isBoosting = false;

    private Vector3 desiredMoveVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (isActiveCharacter)
        {
            CalculateMovement();
            RotateFalcon();
        }
        else
        {
            AnchorToShoulder();
        }
    }

    private void FixedUpdate()
    {
        if (!isActiveCharacter)
            return;

        rb.MovePosition(rb.position + desiredMoveVelocity * Time.fixedDeltaTime);
    }

    private void CalculateMovement()
    {
        Vector3 horizontalMove = new Vector3(moveInput.x, 0f, moveInput.y);
        horizontalMove = Vector3.ClampMagnitude(horizontalMove, 1f);

        Vector3 verticalMove = Vector3.up * verticalInput;

        float horizontalSpeed = isBoosting ? boostSpeed : moveSpeed;

        Vector3 horizontalVelocity = horizontalMove * horizontalSpeed;
        Vector3 verticalVelocity = verticalMove * verticalSpeed;

        desiredMoveVelocity = horizontalVelocity + verticalVelocity;
    }

    private void RotateFalcon()
    {
        Vector3 horizontalMove = new Vector3(moveInput.x, 0f, moveInput.y);

        if (horizontalMove.magnitude < 0.1f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(horizontalMove.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void AnchorToShoulder()
    {
        if (shoulderAnchor == null)
            return;

        desiredMoveVelocity = Vector3.zero;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = Vector3.Lerp(
            transform.position,
            shoulderAnchor.position,
            anchorFollowSpeed * Time.deltaTime
        );

        Quaternion targetRotation = Quaternion.Slerp(
            transform.rotation,
            shoulderAnchor.rotation,
            anchorRotationSpeed * Time.deltaTime
        );

        rb.MovePosition(targetPosition);
        rb.MoveRotation(targetRotation);
    }

    public void SetActiveCharacter(bool active)
    {
        isActiveCharacter = active;

        moveInput = Vector2.zero;
        verticalInput = 0f;
        isBoosting = false;
        desiredMoveVelocity = Vector3.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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