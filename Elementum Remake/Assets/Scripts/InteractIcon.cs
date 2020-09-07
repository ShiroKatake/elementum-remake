using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIcon : MonoBehaviour
{

    public SpriteRenderer icon;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().size = transform.parent.GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Activate(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Activate(false);
        }
    }

    private void Activate(bool active)
    {
        if (active)
        {
            icon.color = new Color(255, 255, 255, 255);
        }
        else
        {
            icon.color = new Color(255, 255, 255, 0);
        }
    }
}
