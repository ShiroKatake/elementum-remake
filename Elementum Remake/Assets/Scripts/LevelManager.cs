using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	//A singleton that handles level loading
	public static LevelManager instance;
	public bool isDone { get; private set; }

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadNextScene() {
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		SceneManager.LoadSceneAsync(nextSceneIndex);
	}
}
