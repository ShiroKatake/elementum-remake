using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCube : MonoBehaviour
{
    Vector2 kickback = new Vector2(25, 10);
    Vector2 polarity;

    public AudioClip land;
    public AudioClip earthBreak;
    public GameObject dustPuff;

    // Start is called before the first frame update
    void Start()
    {
        Hazard.hazardEvent += Break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break(GameObject called)
    {
        if (gameObject == called)
        {
            SoundManager.PlaySound(earthBreak);
            Destroy(gameObject);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player.OnWall())
            {
                player.mountingEarth = true;
            }
            else
            {
                player.mountingEarth = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerController>().playerPosition == Position.Air)
            {
                gameObject.transform.SetParent(collision.transform);
            }
        }

        if (collision.gameObject.layer == 8)
        {
            DustPuff();
            SoundManager.PlaySound(land);
        }
    }

    public void DustPuff()
    {
        GameObject puff = Instantiate(dustPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.transform.SetParent(null);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        
        PlayerController script = collision.GetComponent<PlayerController>();
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (collision.gameObject.tag == "Player")
        {
            if (script.mountingEarth && script.jump.wallJumped)
            {
                Vector2 polarity = new Vector2();
                if (rb.velocity.x < 0)
                {
                    polarity.x = -1;
                }
                else
                {
                    polarity.x = 1;
                }
                if (rb.velocity.y < 0)
                {
                    polarity.y = -1;
                }
                else
                {
                    polarity.y = 1;
                }
                GetComponent<Rigidbody2D>().velocity = rb.velocity - (polarity*kickback);
                
            }
        }
    }
}
