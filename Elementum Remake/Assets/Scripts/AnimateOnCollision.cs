using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnCollision : MonoBehaviour
{
    public AudioClip rustle;
    public float soundCooldown;

    public Animator anim;
    public SpriteRenderer render;
    public bool vertical;
    public float playerVelocity;

    private void Start()
    {
        if (transform.rotation.z == 90)
        {
            vertical = true;
        }
    }

    private void Update()
    {
        soundCooldown -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (vertical)
            {
                playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity.y;
            }
            else
            {
                playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            }
        
       
            if (render.flipX)
            {
                playerVelocity = -1 * playerVelocity;
            }

            anim.SetFloat("PlayerVelocity", playerVelocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (soundCooldown < 0)
        {
            soundCooldown = 0.15f;
            SoundManager.PlaySound(rustle);
        }
    }
}
