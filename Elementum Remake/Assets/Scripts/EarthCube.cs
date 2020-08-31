using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthCube : MonoBehaviour
{
    Vector2 kickback = new Vector2(25, 10);
    Vector2 polarity;

    public AudioSource pushSound;
    public AudioClip land;
    public AudioClip earthBreak;
    public GameObject dustPuff;
    public Rigidbody2D rb;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        Hazard.hazardEvent += Break;
        SceneManager.sceneUnloaded += ctx => DestroySelf();
    }

    // Update is called once per frame
    void Update()
    {

        if (grounded && (rb.velocity.x < -1 || rb.velocity.x > 1))
        {
            if (!pushSound.isPlaying)
            {
                pushSound.Play();
            }
        }
        else
        {
            pushSound.Stop();
        }
    }

    public void Break(GameObject called)
    {
        if (gameObject == called)
        {
            
            SoundManager.PlaySound(earthBreak);
            DestroySelf();

        }
    }

    public void DestroySelf()
    {
        Hazard.hazardEvent -= Break;
        Destroy(gameObject);
    }
    public void DustPuff()
    {
        GameObject puff = Instantiate(dustPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!grounded)
            {
                collision.GetComponent<PlayerController>().mountingEarthInAir = true;

            }
        }
        if (collision.gameObject.layer == 8)
        {
            DustPuff();
            SoundManager.PlaySound(land);
            grounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        
        PlayerController script = collision.GetComponent<PlayerController>();
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (collision.gameObject.tag == "Player")
        {
            if (script.jump.wallJumped)
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
        if (collision.gameObject.layer == 8)
        {
            grounded = false;
        }
    }
}
