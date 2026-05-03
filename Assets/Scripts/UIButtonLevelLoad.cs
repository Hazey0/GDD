using UnityEngine;
using UnityEngine.SceneManagement;  // ADD THIS LINE


public class UIButtonLevelLoad : MonoBehaviour {
	
	public string LevelToLoad;
	
	public void loadLevel() {
		//Load the level from LevelToLoad
		// OLD (removed): Application.LoadLevel(LevelToLoad);
        SceneManager.LoadScene(LevelToLoad);  // FIXED

	}
}
