using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scene Loading")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Game State")]
    [SerializeField] private bool levelEnded = false;

    [Header("Optional Player References")]
    public HumanController humanController;
    public FalconController falconController;
    public CharacterSwitchManager characterSwitchManager;

    private void Awake()
    {
        if (humanController == null)
            humanController = FindFirstObjectByType<HumanController>();

        if (falconController == null)
            falconController = FindFirstObjectByType<FalconController>();

        if (characterSwitchManager == null)
            characterSwitchManager = FindFirstObjectByType<CharacterSwitchManager>();
    }

    public void CompleteLevel()
    {
        if (levelEnded)
            return;

        levelEnded = true;

        Debug.Log("Level complete. Loading main menu.");

        StopPlayerControl();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void StopPlayerControl()
    {
        if (humanController != null)
        {
            humanController.SetActiveCharacter(false);
        }

        if (falconController != null)
        {
            falconController.SetActiveCharacter(false);
        }

        if (characterSwitchManager != null)
        {
            characterSwitchManager.enabled = false;
        }
    }

    public bool HasLevelEnded()
    {
        return levelEnded;
    }
}