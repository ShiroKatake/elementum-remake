using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Animator anim;
    public bool playerInBounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInBounds = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInBounds = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Interaction.eventTrigger += Activate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(int id)
    {
        if(id == 1 && playerInBounds)
        {
            anim.SetTrigger("Active");
        }
    }
}
