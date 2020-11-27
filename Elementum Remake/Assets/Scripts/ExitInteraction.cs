using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitInteraction : MonoBehaviour {

	public delegate void EnterDoor(Vector2 coordinates, string scene);
	public static event EnterDoor doorEvent;

	bool playerInBounds;			//Check if player is overlapping with the exit
	bool isTransitioning;			//Check if the scene is transitioning to stop function from repeating
	Animator anim;					//Animator component of the LevelTransition child
	public string destinationScene;	//The scene the door links to
	public Vector2 destinationDoor; //The position of the linked door
	public PlayerController player;
	public AudioClip enterDoor;
	public SpriteRenderer icon;
	public SpriteRenderer doorLock;
	public bool locked;

	public TMP_Text text;

	private void Awake() {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		PlayerController.playerInteract += PlayerInteract;
		icon = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		doorLock = GetComponent<SpriteRenderer>();
	}

	void Start() {
		DisplayIcon(0);
	}

	void Update() {

		if (locked)
		{
			doorLock.color = new Color(255, 255, 255, 1);

		}
		else
		{
			doorLock.color = new Color(255, 255, 255, 0);
		}

		
	}

	private void PlayerInteract()
	{
		if (!isTransitioning && playerInBounds)
		{
			if (locked)
			{
			}
			else
			{
				isTransitioning = true;
				StartCoroutine(ResetTransitioning());
				SoundManager.PlaySound(enterDoor);
				doorEvent?.Invoke(destinationDoor, destinationScene);
			}

		}
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

	private IEnumerator ResetTransitioning()
	{
		yield return new WaitForSeconds(2);
		isTransitioning = false;
	}

	//Change the alpha of the interact icon to appear when in bounds and disappear when out of bounds
	private void DisplayIcon(float alpha)
	{
		Color tmp = icon.color;
		tmp.a = alpha;
		icon.color = tmp;
	}
}
