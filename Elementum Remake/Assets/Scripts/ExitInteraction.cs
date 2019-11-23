using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitInteraction : MonoBehaviour {
	public int nextSceneIndex;
	private bool playerInBounds;

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
			if (Input.GetButtonDown("Interact")) {
				SceneManager.LoadScene(nextSceneIndex);
			}
		} else {
			Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
			tmp.a = 0f;
			transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		playerInBounds = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		playerInBounds = false;
	}
}
