using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool moving;
    public Vector2 velocityOffset;
    public BoxCollider2D hitbox;
    public SpriteRenderer render;
    

    public void Update()
    {
        if (velocityOffset != Vector2.zero)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    private void Start()
    {
        hitbox.size = render.size;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            //player.GetComponent<Rigidbody2D>().velocity = velocityOffset;

            if (moving && player.wall.mounted && player.Position != Position.Air)
            {
                
                player.wall.joint.enabled = true;
                player.wall.joint.anchor = collision.contacts[0].point;
                player.wall.joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }
}
