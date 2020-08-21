using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public delegate void interactDelegate(bool active, int id);
    public delegate void eventDelegate(int id);
    public static event interactDelegate interactEvent;
    public static event eventDelegate eventTrigger;

    public int id;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactEvent.Invoke(true, id);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().input.Gameplay.Interact.phase == UnityEngine.InputSystem.InputActionPhase.Started)
            {
                eventTrigger.Invoke(id);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactEvent.Invoke(false, id);
        }
    }
}
