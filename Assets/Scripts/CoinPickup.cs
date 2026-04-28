using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public bool canHumanCollect = true;
    public bool canFalconCollect = true;

    [Header("References")]
    public CoinManager coinManager;

    [Header("Visual")]
    public float rotationSpeed = 120f;

    private bool collected = false;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        HumanController human = other.GetComponent<HumanController>();
        FalconController falcon = other.GetComponent<FalconController>();

        bool touchedByHuman = human != null;
        bool touchedByFalcon = falcon != null;

        if (touchedByHuman && !canHumanCollect)
            return;

        if (touchedByFalcon && !canFalconCollect)
            return;

        if (!touchedByHuman && !touchedByFalcon)
            return;

        collected = true;

        if (coinManager != null)
        {
            coinManager.CollectCoin();
        }
        else
        {
            Debug.LogWarning("CoinPickup has no CoinManager assigned.");
        }

        gameObject.SetActive(false);
    }
}