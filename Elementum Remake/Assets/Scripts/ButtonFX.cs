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
        SoundManager.PlaySound(hover);
    }

    public void ClickSound()
    {
        SoundManager.PlaySound(click);
    }

    public void PlaySound()
    {
        SoundManager.PlaySound(play);
    }
}
