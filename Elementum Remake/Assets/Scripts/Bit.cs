using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bit : DestroyOnLoad
{
    public delegate void CollectDelegate();
    public static event CollectDelegate collectEvent;
    public GameObject bitPuff;

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
            SoundManager.PlaySound(collect, 0.5f);
            collectEvent?.Invoke();
                GameObject puff = Instantiate(bitPuff);
                puff.transform.position = gameObject.transform.position;

            // Make sure this is the last thing to happen as it will destroy the object
            Collect();
            
            }
        }
    }
}
