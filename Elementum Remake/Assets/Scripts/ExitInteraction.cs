using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitInteraction : MonoBehaviour {
	bool playerInBounds;			//Check if player is overlapping with the exit
	bool isTransitioning;			//Check if the scene is transitioning to stop function from repeating
	Animator anim;					//Animator component of the LevelTransition child
	public string destinationScene;	//The scene the door links to
	public Vector2 destinationDoor; //The position of the linked door
	public PlayerMovement player;
	public bool locked;
	public TMP_Text text;

	private void Awake() {
		anim = GetComponentInChildren<Animator>();
		anim.SetTrigger("FadeIn");
	}

	void Start() {
		DisplayIcon(0);
	}

	void Update() {
		if (Input.GetButtonDown("Open"))
		{
			locked = false;
		}


		if (locked)
		{
			GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);

		}
		else
		{
			GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
		}

		if (Input.GetButtonDown("Interact") && !isTransitioning && playerInBounds) {
			if (locked)
			{

			}
			else
			{
				isTransitioning = true;

				player.Freeze();

				Debug.Log(destinationDoor.x + " : " + destinationDoor.y);
				StartCoroutine(TransitionToNextScene());
			}
			
		}
	}
	
	//changes scene with simple animation
	private IEnumerator TransitionToNextScene() {

		//Instant scene switching doesn't feel as good as having half a second breather for the eyes
		float timePadding = 0.5f;
		anim.SetTrigger("FadeOut");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timePadding);
		isTransitioning = false;

		//Saves Scene Data to GameData
		GameData.queue.Clear();
		player.queue.Save();
		
		GameData.spawnLocation = destinationDoor;

		//Change Scene
		SceneManager.LoadScene(destinationScene);
	}

	//Player has entered bounds
	void OnTriggerEnter2D(Collider2D other) {
		playerInBounds = true;
		DisplayIcon(1);
	}

	//Player has left bounds
	void OnTriggerExit2D(Collider2D other) {
		playerInBounds = false;
		DisplayIcon(0);
	}

	//Change the alpha of the interact icon to appear when in bounds and disappear when out of bounds
	private void DisplayIcon(float alpha)
	{
		Color tmp = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
		tmp.a = alpha;
		transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tmp;
	}
}
