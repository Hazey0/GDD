using UnityEngine;

public class FalconPerchInteractable : MonoBehaviour
{
    [Header("References")]
    public Transform falconPerchPoint;

    [Header("Gate")]
    public FalconPerchGate linkedGate;

    [Header("State")]
    public bool falconCurrentlyPerchedHere = false;

    private void Awake()
    {
        if (falconPerchPoint == null)
        {
            Transform foundPoint = transform.Find("FalconPerchPoint");

            if (foundPoint != null)
            {
                falconPerchPoint = foundPoint;
            }
            else
            {
                Debug.LogWarning(name + " is missing FalconPerchPoint.");
            }
        }
    }

    public Transform GetPerchPoint()
    {
        return falconPerchPoint;
    }

    public bool HasFalconPerchedHere()
    {
        return falconCurrentlyPerchedHere;
    }

    public void SetFalconPerchedHere(bool perched)
    {
        falconCurrentlyPerchedHere = perched;

        if (linkedGate == null)
            return;

        if (falconCurrentlyPerchedHere)
        {
            linkedGate.OpenGate();
        }
        else
        {
            linkedGate.CloseGate();
        }
    }
}