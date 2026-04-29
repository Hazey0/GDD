using UnityEngine;

public class HumanWinTriggerDetector : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;

    private bool hasWon = false;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryWin(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryWin(other);
    }

    private void TryWin(Collider other)
    {
        if (hasWon)
            return;

        WinZone winZone = other.GetComponent<WinZone>();

        if (winZone == null)
            return;

        hasWon = true;

        Debug.Log("Human entered WinZone trigger.");

        if (gameManager != null)
        {
            gameManager.WinGame();
        }
        else
        {
            Debug.LogWarning("HumanWinTriggerDetector could not find GameManager.");
        }
    }
}