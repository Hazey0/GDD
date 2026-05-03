using UnityEngine;
using UnityEngine.SceneManagement;  // ADD THIS LINE

public class TriggerLevelLoad : MonoBehaviour {

	public string nameOfLevelToLoad  = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.tag == "Player" )
		{
			// OLD (removed): Application.LoadLevel(nameOfLevelToLoad);
            SceneManager.LoadScene(nameOfLevelToLoad);  // FIXED

		}
	}
}
