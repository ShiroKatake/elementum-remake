using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEvent : MonoBehaviour
{
    public Song areaMusic;
    public Song musicOnEntry;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            musicOnEntry = SoundManager.nextSong;
            SoundManager.ChangeMusic(areaMusic, false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.ChangeMusic(musicOnEntry, false);
        }
    }
}
