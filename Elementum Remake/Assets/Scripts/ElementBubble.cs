using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBubble : MonoBehaviour {

	public PlayerAbility player;
	public Animator anim;
	public AudioClip collect;
	public AudioClip turnOn;

	public string room;
	public string element;
	public bool active;

	public bool nodeActive;

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerAbility>();
		Interaction.eventTrigger += Activate;
		room = transform.parent.name;
	}

	private void Activate(int id)
	{
		if(id == 1)
		{
			nodeActive = true;
		}
	}

	public void Update()
	{
		if (nodeActive && CameraController.target.name == room)
		{
			active = true;
		}
		anim.SetBool("Active", active);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!player.queue.Full && active) {
			anim.SetTrigger("Collect");
			SoundManager.PlaySound(collect);
			player.queue.AddElement(element);
		}
	}

	public void TurnOn()
	{
		SoundManager.PlaySound(turnOn, 0.5f);
	}
}
