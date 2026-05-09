using UnityEngine;

public class FalconPerchGate : MonoBehaviour
{
    [Header("Gate Parts")]
    public GameObject gateVisualAndCollider;

    [Header("State")]
    public bool startClosed = true;

    private bool isOpen = false;

    private void Awake()
    {
        if (gateVisualAndCollider == null)
        {
            gateVisualAndCollider = gameObject;
        }
    }

    private void Start()
    {
        if (startClosed)
        {
            CloseGate();
        }
        else
        {
            OpenGate();
        }
    }

    public void OpenGate()
    {
        isOpen = true;

        if (gateVisualAndCollider != null)
        {
            gateVisualAndCollider.SetActive(false);
        }

        Debug.Log(name + " disappeared because falcon is perched.");
    }

    public void CloseGate()
    {
        isOpen = false;

        if (gateVisualAndCollider != null)
        {
            gateVisualAndCollider.SetActive(true);
        }

        Debug.Log(name + " reappeared because falcon left the perch.");
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}