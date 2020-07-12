using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("leaving collision");
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().mountingEarth && collision.gameObject.GetComponent<PlayerMovement>().wallJumped)
            {
                GetComponent<Rigidbody2D>().velocity = -collision.gameObject.GetComponent<Rigidbody2D>().velocity/2;
            }
        }
    }
}
