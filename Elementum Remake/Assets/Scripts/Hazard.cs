using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public AudioClip death;
    public AudioClip earthBreak;
    public float playerCooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Earth")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            //because the player has 2 capsules at the bottom, it will trigger this twice without the cooldown
            if (playerCooldown <= 0)
            {
                playerCooldown = 1f;
                PlayerController.player.Die();
            }
        }
    }

    private void Update()
    {
        playerCooldown -= Time.deltaTime;
    }
}
