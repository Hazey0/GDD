using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Gate Settings")]
    public bool startClosed = true;
    public float openMoveDistance = 4f;
    public float openSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openMoveDistance;

        if (startClosed)
        {
            isOpen = false;
            transform.position = closedPosition;
        }
        else
        {
            isOpen = true;
            transform.position = openPosition;
        }
    }

    private void Update()
    {
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            openSpeed * Time.deltaTime
        );
    }

    public void OpenGate()
    {
        isOpen = true;
    }

    public void CloseGate()
    {
        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}