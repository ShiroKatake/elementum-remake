using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static bool spawned = false;
    private AudioSource sound;
    public AudioSource tarrinsTheme;
    public AudioSource puzzleTheme;
    public AudioSource viollsTheme;
    public AudioSource menuTheme;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            SceneManager.activeSceneChanged += ChangeMusic;
            DontDestroyOnLoad(gameObject);
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

    public void ChangeMusic(Scene current, Scene next)
    {
        if (next.buildIndex == 1 || next.buildIndex == 0)
        {
            return;
        }
        StopMusic();
        switch (next.buildIndex)
        {
            case 2:
                tarrinsTheme.Play();
                break;
            case 3:
                viollsTheme.Play();
                break;
        }
    }

    public void StopMusic()
    {
        tarrinsTheme.Stop();
        puzzleTheme.Stop();
        viollsTheme.Stop();
        menuTheme.Stop();
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
