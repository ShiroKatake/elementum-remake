using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bit : DestroyOnLoad
{
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
            Collect();
            GameData.bits++;
        }
    }
}
