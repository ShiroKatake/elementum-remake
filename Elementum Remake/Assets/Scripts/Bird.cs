using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector2 speed;

    public AudioClip flapA;
    public AudioClip flapB;
    public AudioClip flapC;
    public AudioClip flapD;
    public AudioClip chirp;

    public Animator anim;
    public SpriteRenderer render;
    public bool spooked;
    public float targetHeight;
    public float spookCooldown;
    public int direction;
    public bool grounded;
    public SimpleTrigger spookRange;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spookRange.triggered)
        {
            spooked = true;
            spookCooldown = Random.Range(10, 20);
            targetHeight = transform.position.y + Random.Range(5, 10);
            direction = spookRange.direction;
            if (direction == 1)
            {
                render.flipX = true;
            }
            spookRange.triggered = false;
        }
        if (spooked) 
        { 
            if (spookCooldown >= 0)
            {
                anim.SetBool("Flying", true);
                Move();
                spookCooldown -= Time.deltaTime;
            }
            else
            {
                spooked = false;
            }
        }
        
    }

    public void Ascend()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + speed.y);
    }

    public void Decend()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + speed.y);
    }

    public void Move()
    {
        transform.position = new Vector2(transform.position.x + (direction* speed.x), transform.position.y);
        if (transform.position.y < targetHeight)
        {
            anim.SetBool("Flapping", true);
            Ascend();
        }
        else
        {
            anim.SetBool("Flapping", false);
        }
    }

    public void Flap()
    {
        switch (Random.Range(2, 3))
        {
            case 0:
                SoundManager.PlaySound(flapA, 0.5f);
                break;
            case 1:
                SoundManager.PlaySound(flapB, 0.5f);
                break;
            case 2:
                SoundManager.PlaySound(flapC, 0.5f);
                break;
            case 3:
                SoundManager.PlaySound(flapD, 0.5f);
                break;
        }
    }

    public void Chirp()
    {
        SoundManager.PlaySound(chirp, 0.6f);
    }
}
