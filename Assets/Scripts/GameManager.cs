using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private bool gameEnded = false;

    [Header("Reset")]
    public float resetDelay = 3f;

    [Header("UI")]
    public GameObject winPanel;
    public TMP_Text winText;

    [Header("Player References")]
    public HumanController humanController;
    public FalconController falconController;

    [Header("Optional Managers")]
    public CharacterSwitchManager characterSwitchManager;

    private void Awake()
    {
        // Auto-find references if you forgot to drag them in.
        if (humanController == null)
            humanController = FindFirstObjectByType<HumanController>();

        if (falconController == null)
            falconController = FindFirstObjectByType<FalconController>();

        if (characterSwitchManager == null)
            characterSwitchManager = FindFirstObjectByType<CharacterSwitchManager>();

        // Make sure win UI is hidden at the start.
        if (winPanel != null)
            winPanel.SetActive(false);

        if (winText != null)
            winText.text = "";
    }

    public void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        Debug.Log("You Win!");

        // Show win UI.
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameManager: Win Panel is not assigned.");
        }

        if (winText != null)
        {
            winText.text = "LEVEL COMPLETE!";
        }
        else
        {
            Debug.LogWarning("GameManager: Win Text is not assigned.");
        }

        // Stop both characters from receiving control.
        if (humanController != null)
        {
            humanController.SetActiveCharacter(false);
        }
        else
        {
            Debug.LogWarning("GameManager: Human Controller is not assigned.");
        }

        if (falconController != null)
        {
            falconController.SetActiveCharacter(false);
        }
        else
        {
            Debug.LogWarning("GameManager: Falcon Controller is not assigned.");
        }

        Invoke(nameof(RestartScene), resetDelay);

    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool HasGameEnded()
    {
        return gameEnded;
    }
}