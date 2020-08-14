using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCube : MonoBehaviour
{
    Vector2 kickback = new Vector2(25, 10);
    Vector2 polarity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerJump>().mountingEarth = true;
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
                transform.SetParent(null);
                GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity - (polarity*kickback);
                
            }
        }
        script.mountingEarth = false;
    }
}
