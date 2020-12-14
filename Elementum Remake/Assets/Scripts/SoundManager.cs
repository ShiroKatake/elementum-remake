using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static bool spawned = false;
    private AudioSource sound;
    public static AudioSource ambience;
    public Song currentSong;
    public static Song nextSong;
    
    public 

    // Start is called before the first frame update
    void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);
            nextSong = currentSong;

            

            ambience = GetComponent<AudioSource>();
            ambience.clip = currentSong.clip;
            ambience.Play();
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
        if (currentSong.bar)
        {
            if (currentSong != nextSong)
            {
                currentSong = nextSong;
                ambience.clip = currentSong.clip;
                ambience.Play();
            }
        }
    }

    public static void ChangeMusic(Song song, bool changeImmediate)
    {
        if (changeImmediate)
        {
            ambience.clip = song.clip;
        }
        nextSong = song;
    }

    public void StopMusic()
    {
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

    public static void PlaySound(AudioClip clip, float volume)
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
}
