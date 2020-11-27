using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChaseTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    public bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!triggered)
            {
                timeline.Play();
                triggered = true;
            }
        }
    }
}
