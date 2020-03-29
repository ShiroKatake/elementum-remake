using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBubble : MonoBehaviour {

	public PlayerMovement player;
	public BubbleAnimation bubbleAnimation;
	public RuneAnimation runeAnimation;
	public GameObject element;

	void OnTriggerEnter2D(Collider2D other) {
		if (!player.GetComponent<AbilitySlot>().occupied) {
			bubbleAnimation.animating = true;
			runeAnimation.animating = true;
			player.slot.occupied = true;
			player.slot.element = element;
		}
	}
}
