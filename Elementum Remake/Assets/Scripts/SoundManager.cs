using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static bool spawned = false;
    private AudioSource sound;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (!spawned)
        {
            spawned = true;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            sound = GameObject.Find("Sound").GetComponent<AudioSource>();
            if (!sound.isPlaying)
            {
                Destroy(sound.gameObject);
            }
        }
        catch
        {

        }
    }

    public static void PlaySound(AudioClip clip)
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
