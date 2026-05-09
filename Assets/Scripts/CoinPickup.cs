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

    [Header("Perch Controlled Visibility")]
    public bool controlledByPerch = false;
    public bool visibleWhenPerchInactive = true;

    private bool collected = false;
    private Renderer[] renderers;
    private Collider[] colliders;

    private void Awake()
    {
        if (coinManager == null)
        {
            coinManager = FindFirstObjectByType<CoinManager>();
        }

        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
    }

    private void Start()
    {
        if (controlledByPerch)
        {
            SetVisible(visibleWhenPerchInactive);
        }
    }

    private void Update()
    {
        if (collected)
            return;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        HumanController human = other.GetComponentInParent<HumanController>();
        FalconController falcon = other.GetComponentInParent<FalconController>();

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
            Debug.LogWarning("CoinPickup could not find a CoinManager in the scene.");
        }

        SetVisible(false);
    }

    public void SetVisibleFromPerch(bool visible)
    {
        if (collected)
        {
            SetVisible(false);
            return;
        }

        SetVisible(visible);
    }

    private void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            if (r != null)
            {
                r.enabled = visible;
            }
        }

        foreach (Collider c in colliders)
        {
            if (c != null)
            {
                c.enabled = visible;
            }
        }
    }

    public bool IsCollected()
    {
        return collected;
    }
}