using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCube : MonoBehaviour
{
    Vector2 kickback = new Vector2(25, 10);
    Vector2 polarity;

    public AudioClip land;
    public AudioClip earthBreak;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        SoundManager.PlaySound(earthBreak);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().OnWall(collision.GetComponent<PlayerController>().playerPosition))
            {
                collision.GetComponent<PlayerJump>().mountingEarth = true;
            }
            else
            {
                collision.GetComponent<PlayerJump>().mountingEarth = false;
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
            SoundManager.PlaySound(land);
        }
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
        
        PlayerJump script = collision.GetComponent<PlayerJump>();
        if (collision.gameObject.tag == "Player")
        {
            if (script.mountingEarth && script.wallJumped)
            {
                Vector2 polarity = new Vector2();
                if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    polarity.x = -1;
                }
                else
                {
                    polarity.x = 1;
                }
                if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    polarity.y = -1;
                }
                else
                {
                    polarity.y = 1;
                }
                GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity - (polarity*kickback);
                
            }
        }
    }
}
