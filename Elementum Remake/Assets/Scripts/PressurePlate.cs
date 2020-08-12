using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public SpriteRenderer sprite;
    
    public bool activated;
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Earth")
        {
            activated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Earth")
        {
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
            door.GetComponent<ExitInteraction>().locked = false;

        }
        else
        {
            sprite.sprite = up;
            door.GetComponent<ExitInteraction>().locked = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, door.transform.position);
    }
}
