using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarrinsCape : DestroyOnLoad
{
    public int eventId;

    private void Start()
    {
        Interaction.eventTrigger += Activate;
    }

    public void Activate(int id)
    {
        if (id == eventId)
        {
            gameObject.SetActive(false);
        }
    }
}
