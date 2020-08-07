using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBubble : MonoBehaviour {

	public PlayerMovement player;
	public BubbleAnimation bubbleAnimation;
	public RuneAnimation runeAnimation;

	public AudioClip collect;

	public string element;

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerMovement>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!player.queue.Full) {
			SoundManager.PlaySound(collect);
			bubbleAnimation.animating = true;
			runeAnimation.animating = true;
			player.queue.AddElement(element);
		}
	}
}
