using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public AudioClip death;
    public AudioClip earthBreak;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Earth")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.PlaySound(death, 0.35f);
            PlayerMovement.player.alive = false;
            PlayerMovement.player.disabled = true;
        }
    }
}
