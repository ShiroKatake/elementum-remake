using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitInteraction : MonoBehaviour {
	bool playerInBounds;
	bool isTransitioning;	//Check if the scene is transitioning to stop function from repeating
	Animator anim;          //Animator component of the LevelTransition child
	public string destinationScene;
	public Vector2 destinationDoor;
	public int doorIndex;
	public PlayerMovement player;

	private void Awake() {
		anim = GetComponentInChildren<Animator>();
		anim.SetTrigger("FadeIn");
	}

	void Start() {
		DisplayIcon(0);
	}

	void Update() {
		if (playerInBounds) {
			DisplayIcon(1);
			if (Input.GetButtonDown("Interact") && !isTransitioning) {
				isTransitioning = true;
				
				StartCoroutine(TransitionToNextScene());
			}
		} else {
			DisplayIcon(0);
		}
	}

	private IEnumerator TransitionToNextScene() {
		//Instant scene switching doesn't feel as good as having half a second breather for the eyes
		float timePadding = 0.5f;
		//Fade out current scene, then wait for the transition to finish before loading next scene
		anim.SetTrigger("FadeOut");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timePadding);
		isTransitioning = false;

		player.queue.Save();
		GameData.spawnLocation = destinationDoor;
		SceneManager.LoadScene(destinationScene);
	}

	void OnTriggerEnter2D(Collider2D other) {
		playerInBounds = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		playerInBounds = false;
	}

	private void DisplayIcon(float alpha)
	{
		Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
		tmp.a = alpha;
		transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
	}
}
