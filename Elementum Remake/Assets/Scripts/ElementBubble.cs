﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBubble : MonoBehaviour
{
    public GameObject player;
    public GameObject bubbleAnimation;
    public GameObject runeAnimation;
    public Element element;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player.GetComponent<AbilitySlot>().occupied == false && player.GetComponent<AbilitySlot>().element != element)
            {
                
                player.GetComponent<AbilitySlot>().occupied = true;
                player.GetComponent<AbilitySlot>().element = element;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (player.GetComponent<AbilitySlot>().occupied == false)
        {
            bubbleAnimation.GetComponent<BubbleAnimation>().animating = true;
            runeAnimation.GetComponent<RuneAnimation>().animating = true;
            player.GetComponent<AbilitySlot>().element = null;
        }
    }
}
