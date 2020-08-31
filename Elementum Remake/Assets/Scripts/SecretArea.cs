using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public Animator animator;
    public AudioClip sound;
    public bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("IsRevealed", true);
            if (!triggered)
            {
                triggered = true;
                SoundManager.PlaySound(sound, 0.2f); 
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("IsRevealed", false);
        }
    }
}
