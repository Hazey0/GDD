using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Requirement")]
    public int coinsNeededToCompleteLevel = 1;
    public int currentCoins = 0;

    [Header("References")]
    public GameManager gameManager;

    private bool objectiveCompleted = false;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    public void CollectCoin()
    {
        if (objectiveCompleted)
            return;

        currentCoins++;

        Debug.Log("Coin collected! Coins: " + currentCoins + "/" + coinsNeededToCompleteLevel);

        if (currentCoins >= coinsNeededToCompleteLevel)
        {
            objectiveCompleted = true;

            Debug.Log("Coin requirement met. Completing level.");

            if (gameManager != null)
            {
                gameManager.CompleteLevel();
            }
            else
            {
                Debug.LogWarning("CoinManager has no GameManager assigned.");
            }
        }
    }
}