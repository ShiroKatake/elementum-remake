using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnCollision : MonoBehaviour
{
    public Animator anim;
    public bool vertical;

    private void Start()
    {
        if (transform.rotation.z == 90)
        {
            vertical = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!vertical)
            {
                anim.SetFloat("PlayerVelocity", collision.gameObject.GetComponent<Rigidbody2D>().velocity.x);
            }
            else
            {
                anim.SetFloat("PlayerVelocity", collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
        }
    }
}
