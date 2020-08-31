using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite up;
    public Sprite down;
    public Animator anim;
    
    public bool activated;
    public bool inverted;
    public float activateTimer;
    public bool deactivationStarted;

    public GameObject linkedObject;

    public AudioClip downSound;
    public AudioClip upSound;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Earth")
        {
            activated = true;
            activateTimer = 0.2f;
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
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        activateTimer -= Time.deltaTime;
        if (activated)
        {
            anim.SetBool("Up", false);
            Activate(true);

        }
        else if (activateTimer < 0)
        {
            anim.SetBool("Up", true);
            Activate(false);
        }
    }

    public void Activate(bool active)
    {
        if(inverted)
        {
            linkedObject.GetComponent<ObjectOnRail>().active = !active;
        }
        else
        {
            linkedObject.GetComponent<ObjectOnRail>().active = active;
        }
    }

    public void PlayUpSound()
    {
        SoundManager.PlaySound(upSound, 0.1f);
    } 

    public void PlayDownSound()
    {
        SoundManager.PlaySound(downSound, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, linkedObject.transform.position);
    }
}
