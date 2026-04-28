using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Requirement")]
    public int coinsNeededToOpenGate = 1;
    public int currentCoins = 0;

    [Header("Gate")]
    public Gate gateToOpen;

    public void CollectCoin()
    {
        currentCoins++;

        Debug.Log("Coin collected! Coins: " + currentCoins + "/" + coinsNeededToOpenGate);

        if (currentCoins >= coinsNeededToOpenGate)
        {
            if (gateToOpen != null)
            {
                gateToOpen.OpenGate();
                Debug.Log("Gate opened!");
            }
            else
            {
                Debug.LogWarning("CoinManager has no gate assigned.");
            }
        }
    }
}