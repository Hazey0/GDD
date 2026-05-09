using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Level Flow")]
    public string level1SceneName = "Level1";
    public string level2SceneName = "Level2";
    public string level3SceneName = "Level3";
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

        Debug.Log("Level complete.");

        StopPlayerControl();

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level1")
        {
            Debug.Log("Loading Level 2.");
            SceneManager.LoadScene("Level2");
        }
        else if (currentSceneName == "Level2")
        {
            Debug.Log("Loading Level 3.");
            SceneManager.LoadScene("Level3");
        }
        else if (currentSceneName == "Level3")
        {
            Debug.Log("Level 3 complete. Loading Main Menu.");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("Current scene is not recognized by GameManager: " + currentSceneName);
            SceneManager.LoadScene("MainMenu");
        }
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