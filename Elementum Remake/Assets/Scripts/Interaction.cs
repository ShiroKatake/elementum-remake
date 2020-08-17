using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public delegate void interactDelegate(bool active);
    public delegate void eventDelegate(int id);
    public static event interactDelegate interactEvent;
    public static event eventDelegate eventTrigger;

    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactEvent.Invoke(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Up"))
            {
                eventTrigger.Invoke(id);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactEvent.Invoke(false);
        }
    }
}
