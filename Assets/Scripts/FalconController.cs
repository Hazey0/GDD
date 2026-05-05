using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FalconController : MonoBehaviour
{
    [Header("Flight")]
    public float flightSpeed = 8f;
    public float mouseYawSensitivity = 0.12f;
    public float mousePitchSensitivity = 0.08f;
    public float rotationSmoothSpeed = 10f;
    public float minPitch = -45f;
    public float maxPitch = 80f;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("Shoulder Anchor")]
    public Transform shoulderAnchor;
    public float anchorFollowSpeed = 12f;
    public float anchorRotationSpeed = 12f;

    [Header("Perch")]
    public Transform currentPerchPoint;
    public bool isPerchedAwayFromHuman = false;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody rb;

    private bool isActiveCharacter = false;

    private Vector2 lookInput;
    private Vector3 desiredMoveVelocity;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (cameraFollow == null)
        {
            cameraFollow = FindFirstObjectByType<CameraFollow>();
        }

        yaw = transform.eulerAngles.y;
        pitch = 0f;
    }

    private void Update()
    {
        if (isActiveCharacter)
        {
            HandleMouseSteering();
            CalculateForwardFlight();
            SetAnimationState(true, false);
        }
        else
        {
            desiredMoveVelocity = Vector3.zero;

            if (isPerchedAwayFromHuman && currentPerchPoint != null)
            {
                AnchorToTarget(currentPerchPoint, true);
                SetAnimationState(false, true);
            }
            else
            {
                AnchorToTarget(shoulderAnchor, true);
                SetAnimationState(false, false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isActiveCharacter)
            return;

        rb.MovePosition(rb.position + desiredMoveVelocity * Time.fixedDeltaTime);
    }

    private void HandleMouseSteering()
    {
        yaw += lookInput.x * mouseYawSensitivity;
        pitch -= lookInput.y * mousePitchSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSmoothSpeed * Time.deltaTime
            )
        );
    }

    private void CalculateForwardFlight()
    {
        desiredMoveVelocity = transform.forward * flightSpeed;
    }

    private void AnchorToTarget(Transform target, bool smooth)
    {
        if (target == null)
            return;

        StopRigidbodyMovement();

        if (smooth)
        {

            float distanceToTarget = Vector3.Distance(rb.position, target.position);

            if (distanceToTarget < 0.5f)
            {
                rb.position = target.position;
                rb.rotation = target.rotation;
                return;
            }

            Vector3 targetPosition = Vector3.Lerp(
                rb.position,
                target.position,
                anchorFollowSpeed * Time.deltaTime
            );

            Quaternion targetRotation = Quaternion.Slerp(
                rb.rotation,
                target.rotation,
                anchorRotationSpeed * Time.deltaTime
            );

            rb.MovePosition(targetPosition);
            rb.MoveRotation(targetRotation);
        }
        else
        {
            rb.position = target.position;
            rb.rotation = target.rotation;
        }
    }

    private void StopRigidbodyMovement()
    {
        if (rb == null)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void SetAnimationState(bool flying, bool perched)
    {
        if (animator == null)
            return;

        animator.SetBool("flying", flying);
        animator.SetBool("perched", perched);
    }

    public void SetActiveCharacter(bool active)
    {
        if (active && isPerchedAwayFromHuman)
        {
            Debug.Log("Cannot control falcon while it is perched away from the human.");
            return;
        }

        isActiveCharacter = active;

        lookInput = Vector2.zero;
        desiredMoveVelocity = Vector3.zero;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        if (pitch > 180f)
        {
            pitch -= 360f;
        }

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        StopRigidbodyMovement();

        if (cameraFollow != null)
        {
            cameraFollow.SetFalconMode(active);
        }
    }

    public bool IsActiveCharacter()
    {
        return isActiveCharacter;
    }

    public void PerchAt(Transform perchPoint)
    {
        if (perchPoint == null)
        {
            Debug.LogWarning("FalconController: Tried to perch, but perch point was null.");
            return;
        }

        isActiveCharacter = false;
        isPerchedAwayFromHuman = true;
        currentPerchPoint = perchPoint;

        lookInput = Vector2.zero;
        desiredMoveVelocity = Vector3.zero;

        StopRigidbodyMovement();

        if (cameraFollow != null)
        {
            cameraFollow.SetFalconMode(false);
        }

        Debug.Log("Falcon perched away from human.");
    }

    public void ReturnToShoulder()
    {
        isActiveCharacter = false;
        isPerchedAwayFromHuman = false;
        currentPerchPoint = null;

        lookInput = Vector2.zero;
        desiredMoveVelocity = Vector3.zero;

        StopRigidbodyMovement();

        if (cameraFollow != null)
        {
            cameraFollow.SetFalconMode(false);
        }

        Debug.Log("Falcon returned to human shoulder.");
    }

    public bool IsPerchedAway()
    {
        return isPerchedAwayFromHuman;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!isActiveCharacter)
            return;

        lookInput = context.ReadValue<Vector2>();
    }
}