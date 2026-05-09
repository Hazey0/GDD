using UnityEngine;
using UnityEngine.InputSystem;

public class HumanPerchInteractor : MonoBehaviour
{
    [Header("References")]
    public HumanController humanController;
    public FalconController falconController;

    [Header("Current Interaction")]
    public FalconPerchInteractable currentPerch;

    private FalconPerchInteractable activeFalconPerch;

    private void Awake()
    {
        if (humanController == null)
            humanController = GetComponent<HumanController>();

        if (falconController == null)
            falconController = FindFirstObjectByType<FalconController>();
    }

    public void SetCurrentPerch(FalconPerchInteractable perch)
    {
        currentPerch = perch;

        if (currentPerch != null)
        {
            Debug.Log("Near perch: " + currentPerch.name + ". Press E to interact.");
        }
    }

    public void ClearCurrentPerch(FalconPerchInteractable perch)
    {
        if (currentPerch == perch)
        {
            currentPerch = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (currentPerch == null)
        {
            Debug.Log("No perch nearby.");
            return;
        }

        InteractWithCurrentPerch();
    }

    private void InteractWithCurrentPerch()
    {
        if (humanController == null || falconController == null)
        {
            Debug.LogWarning("HumanPerchInteractor is missing HumanController or FalconController.");
            return;
        }

        if (currentPerch.GetPerchPoint() == null)
        {
            Debug.LogWarning(currentPerch.name + " has no FalconPerchPoint assigned.");
            return;
        }

        if (activeFalconPerch == currentPerch)
        {
            RetrieveFalcon();
        }
        else
        {
            SendFalconToPerch(currentPerch);
        }
    }

    private void SendFalconToPerch(FalconPerchInteractable perch)
    {
        if (activeFalconPerch != null)
        {
            activeFalconPerch.SetFalconPerchedHere(false);
        }

        activeFalconPerch = perch;
        activeFalconPerch.SetFalconPerchedHere(true);

        falconController.PerchAt(perch.GetPerchPoint());
        humanController.setFalconWithHuman(false);

        Debug.Log("Falcon perched at: " + perch.name + ". Human lost dash and double jump.");
    }

    private void RetrieveFalcon()
    {
        if (activeFalconPerch != null)
        {
            activeFalconPerch.SetFalconPerchedHere(false);
        }

        activeFalconPerch = null;

        falconController.ReturnToShoulder();
        humanController.setFalconWithHuman(true);

        Debug.Log("Falcon retrieved. Human regained dash and double jump.");
    }

    public bool IsFalconCurrentlyPerched()
    {
        return activeFalconPerch != null;
    }
}