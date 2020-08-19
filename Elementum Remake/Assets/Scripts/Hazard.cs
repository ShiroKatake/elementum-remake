using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public AudioClip death;
    public AudioClip earthBreak;
    public float playerCooldown;

    public BoxCollider2D hitBox;
    public SpriteRenderer render;

    public void Start()
    {
        hitBox.size = new Vector2(render.size.x-0.3f, render.size.y/2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Earth")
        {
            collision.GetComponent<EarthCube>().Break();
            
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
