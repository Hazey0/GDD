using UnityEngine;

public class PerchInteractionTrigger : MonoBehaviour
{
    [Header("Reference")]
    public FalconPerchInteractable perchInteractable;

    private void Awake()
    {
        if (perchInteractable == null)
        {
            perchInteractable = GetComponentInParent<FalconPerchInteractable>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HumanPerchInteractor interactor = other.GetComponentInParent<HumanPerchInteractor>();

        if (interactor == null)
            return;

        interactor.SetCurrentPerch(perchInteractable);
    }

    private void OnTriggerExit(Collider other)
    {
        HumanPerchInteractor interactor = other.GetComponentInParent<HumanPerchInteractor>();

        if (interactor == null)
            return;

        interactor.ClearCurrentPerch(perchInteractable);
    }
}