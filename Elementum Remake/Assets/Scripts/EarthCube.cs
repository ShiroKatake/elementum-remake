using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCube : MonoBehaviour
{
    Vector2 kickback = new Vector2(20, 10);
    Vector2 polarity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement script = collision.gameObject.GetComponent<PlayerMovement>();
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(script.wallJumped);
            if (script.mountingEarth && script.wallJumped)
            {
                Debug.Log("React");
                if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.x < 0)
                {
                    polarity = new Vector2(-1, 0);
                }
                else
                {
                    polarity = new Vector2(1, 0);
                }
                GetComponent<Rigidbody2D>().velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity - (polarity*kickback);
                collision.GetComponent<PlayerMovement>().mountingEarth = false;
            }
        }
    }
}
