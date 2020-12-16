using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChaseTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    public bool triggered;
    public bool ended;

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

    public void ManualPlay()
    {
        timeline.Play();
    }

    public void SequenceEnd()
    {
        ended = true;
    }
}
