using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarrinsCape : DestroyOnLoad
{
    public int eventId;
    public AudioClip collect;
    public GameObject tip;

    private void Start()
    {
        Interaction.eventTrigger += Activate;
    }

    public void Activate(int id)
    {
        if (id == eventId)
        {
            tip.SetActive(true);
            SoundManager.PlaySound(collect, 0.3f);
            gameObject.SetActive(false);
        }
    }
}
