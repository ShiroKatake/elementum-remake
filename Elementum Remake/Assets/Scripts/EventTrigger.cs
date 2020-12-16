using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public bool activated;
    public int eventId;
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
        if(id == eventId && playerInBounds && !activated)
        {
            activated = true;
            anim.SetTrigger("Active");
        }
    }
}
