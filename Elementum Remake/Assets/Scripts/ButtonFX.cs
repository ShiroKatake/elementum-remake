using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFX : MonoBehaviour
{
    public AudioClip hover;
    public AudioClip click;
    public AudioClip play;

    public void HoverSound()
    {
        CreateSound(hover, 1);
    }

    public void ClickSound()
    {
        CreateSound(click, 1);
    }

    public void PlaySound()
    {
        CreateSound(play, 0.5f);
    }

    public void CreateSound(AudioClip sound, float volume)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            SoundManager.PlaySound(sound, volume);
            Time.timeScale = 0;
        }
        else
        {
            SoundManager.PlaySound(sound, volume);
        }
    }
}
