using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBubble : MonoBehaviour {

	public PlayerAbility player;

	public Animator anim;

	public AudioClip collect;

	public string element;
	public bool active;

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerAbility>();
	}

	public void Update()
	{
		anim.SetBool("Active", active);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!player.queue.Full) {
			anim.SetTrigger("Collect");
			SoundManager.PlaySound(collect);
			player.queue.AddElement(element);
		}
	}
}
