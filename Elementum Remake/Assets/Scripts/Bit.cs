using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bit : DestroyOnLoad
{
    public delegate void CollectDelegate();
    public static event CollectDelegate collectEvent;

    public AudioClip collect;
    public bool isCollected;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        spawn = transform.position;
        DestroyCollected();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isCollected)
            {
            SoundManager.PlaySound(collect);
            Collect();
            collectEvent?.Invoke();
            }
        }
    }
}
