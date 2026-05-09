using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Human Orbit Camera")]
    public float humanDistance = 8f;
    public float humanHeightOffset = 1.5f;
    public float mouseSensitivity = 0.12f;
    public float minPitch = -20f;
    public float maxPitch = 55f;

    [Header("Falcon Follow Camera")]
    public float falconDistanceBehind = 7f;
    public float falconHeightOffset = 1.5f;

    [Header("Falcon Rear View")]
    public float falconRearViewDistance = 6f;
    public float falconRearViewHeightOffset = 1.5f;

    [Header("Smoothing")]
    public float humanPositionSmoothSpeed = 12f;
    public float humanRotationSmoothSpeed = 12f;
    public float falconPositionSnapSpeed = 30f;
    public float falconRotationSnapSpeed = 30f;

    private float yaw;
    private float pitch = 20f;
    private Vector2 lookInput;

    private bool falconMode = false;
    private bool rearViewHeld = false;

    private void Start()
    {
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        if (falconMode)
        {
            UpdateFalconCamera();
        }
        else
        {
            UpdateHumanCamera();
        }
    }

    private void UpdateHumanCamera()
    {
        rearViewHeld = false;

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Vector3 targetCenter = target.position + Vector3.up * humanHeightOffset;

        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 desiredPosition =
            targetCenter - orbitRotation * Vector3.forward * humanDistance;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            humanPositionSmoothSpeed * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(targetCenter - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            humanRotationSmoothSpeed * Time.deltaTime
        );
    }

    private void UpdateFalconCamera()
    {
        Vector3 targetCenter;

        if (rearViewHeld)
        {
            targetCenter = target.position + Vector3.up * falconRearViewHeightOffset;

            Vector3 desiredPosition =
                target.position +
                target.forward * falconRearViewDistance +
                Vector3.up * falconRearViewHeightOffset;

            

            Quaternion desiredRotation = Quaternion.LookRotation(targetCenter - transform.position);
            SnapCamera(desiredPosition, desiredRotation);
        }
        else
        {
            targetCenter = target.position + Vector3.up * falconHeightOffset;

            Vector3 desiredPosition =
                target.position -
                target.forward * falconDistanceBehind +
                Vector3.up * falconHeightOffset;

            

            Quaternion desiredRotation = Quaternion.LookRotation(targetCenter - transform.position);
            SnapCamera(desiredPosition, desiredRotation);
        }
    }

    private void MoveCameraFast(Vector3 desiredPosition)
    {
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            falconPositionSnapSpeed * Time.deltaTime
        );
    }

    private void RotateCameraFast(Quaternion desiredRotation)
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            falconRotationSnapSpeed * Time.deltaTime
        );
    }

    private void SnapCamera(Vector3 desiredPosition, Quaternion desiredRotation)
    {
        transform.position = desiredPosition;
        transform.rotation = desiredRotation;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target != null && !falconMode)
        {
            Vector3 flatForward = target.forward;
            flatForward.y = 0f;

            if (flatForward.sqrMagnitude > 0.001f)
            {
                yaw = Quaternion.LookRotation(flatForward).eulerAngles.y;
            }
        }
    }

    public void SetFalconMode(bool active)
    {
        falconMode = active;

        if (!falconMode)
        {
            rearViewHeld = false;

            if (target != null)
            {
                Vector3 flatForward = target.forward;
                flatForward.y = 0f;

                if (flatForward.sqrMagnitude > 0.001f)
                {
                    yaw = Quaternion.LookRotation(flatForward).eulerAngles.y;
                }
            }
        }
    }

    public bool IsFalconMode()
    {
        return falconMode;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnRearView(InputAction.CallbackContext context)
    {
        if (!falconMode)
        {
            rearViewHeld = false;
            return;
        }

        if (context.performed)
        {
            rearViewHeld = true;
        }

        if (context.canceled)
        {
            rearViewHeld = false;
        }
    }
}