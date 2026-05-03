// using UnityEngine;
// using System.Collections;


// public class UIButtonQuitGame : MonoBehaviour {

// 	public void quitGame()
// 	{
// 		//Closes the game
// 		Application.Quit();
// 	}
// }

using UnityEngine;

public class UIButtonQuitGame : MonoBehaviour
{
	public void quitGame()
	{
		// This line prints to the console, so you know the button was clicked.
		Debug.Log("Quit Game button pressed!");

		// --- This is the platform-specific code ---

		// If we are running in a standalone build of the game
#if UNITY_STANDALONE
        // Quit the application
        Application.Quit();
#endif

		// If we are running in the Unity Editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
