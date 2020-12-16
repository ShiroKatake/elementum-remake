using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarrinsCape : DestroyOnLoad
{
    public int eventId;
    public GameObject playerCape;
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
            SoundManager.PlaySound(collect, 0.2f);
            playerCape.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
