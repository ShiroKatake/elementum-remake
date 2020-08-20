using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public SpriteRenderer sprite;
    
    public bool activated;
    public GameObject linkedObject;

    public AudioClip downSound;
    public AudioClip upSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Earth")
        {
            if (!activated)
            {
                SoundManager.PlaySound(downSound, 0.2f);
            }
            activated = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Earth")
        {
            if (activated)
            {
                SoundManager.PlaySound(upSound, 0.2f);
            }
            activated = false;
            
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (activated)
        {
            sprite.sprite = down;
            linkedObject.GetComponent<ObjectOnRail>().active = true;

        }
        else
        {
            sprite.sprite = up;
            linkedObject.GetComponent<ObjectOnRail>().active = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, linkedObject.transform.position);
    }
}
