using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [Header("Coin Requirement")]
    public int coinsNeededToOpenGate = 1;
    public int currentCoins = 0;
    public TMP_Text coinText;

    [Header("Gate")]
    public Gate gateToOpen;

    public void CollectCoin()
    {
        currentCoins++;
        coinText.text = "Coins: "+ currentCoins.ToString();

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