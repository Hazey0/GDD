using UnityEngine;

public class FalconPerchInteractable : MonoBehaviour
{
    [Header("References")]
    public Transform falconPerchPoint;

    [Header("Optional Reward Coin")]
    public CoinPickup linkedRewardCoin;
    public bool showCoinOnlyWhenFalconPerched = true;

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

        UpdateLinkedCoinVisibility();
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
        UpdateLinkedCoinVisibility();
    }

    private void UpdateLinkedCoinVisibility()
    {
        if (linkedRewardCoin == null)
            return;

        bool shouldShowCoin;

        if (showCoinOnlyWhenFalconPerched)
        {
            shouldShowCoin = falconCurrentlyPerchedHere;
        }
        else
        {
            shouldShowCoin = !falconCurrentlyPerchedHere;
        }

        linkedRewardCoin.SetVisibleFromPerch(shouldShowCoin);
    }
}