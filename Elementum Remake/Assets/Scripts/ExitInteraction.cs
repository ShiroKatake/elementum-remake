using UnityEngine;
using System.Collections;

public class ExitInteraction : MonoBehaviour {
	bool playerInBounds;
	bool isTransitioning;	//Check if the scene is transitioning to stop function from repeating
	Animator anim;			//Animator component of the LevelTransition child

	private void Awake() {
		anim = GetComponentInChildren<Animator>();
		anim.SetTrigger("FadeIn");
	}

	void Start() {
		Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
		tmp.a = 0f;
		transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
	}

	void Update() {
		if (playerInBounds) {
			Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
			tmp.a = 1f;
			transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
			if (Input.GetButtonDown("Interact") && !isTransitioning) {
				isTransitioning = true;
				StartCoroutine(TransitionToNextScene());
			}
		} else {
			Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
			tmp.a = 0f;
			transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
		}
	}

	private IEnumerator TransitionToNextScene() {
		//Instant scene switching doesn't feel as good as having half a second breather for the eyes
		float timePadding = 0.5f;
		//Fade out current scene, then wait for the transition to finish before loading next scene
		anim.SetTrigger("FadeOut");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timePadding);
		isTransitioning = false;
		LevelManager.instance.LoadNextScene();
	}

	void OnTriggerEnter2D(Collider2D other) {
		playerInBounds = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		playerInBounds = false;
	}
}
