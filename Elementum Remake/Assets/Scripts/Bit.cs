﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bit : MonoBehaviour
{
    public static List<string> collectedBits = new List<string>();

    public void Awake()
    {
        foreach (string b in collectedBits)
        {
            if (b == SceneManager.GetActiveScene().buildIndex + transform.position.x.ToString() + transform.position.y.ToString())
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var pos = transform.position;
            collectedBits.Add(SceneManager.GetActiveScene().buildIndex + pos.x.ToString() + pos.y.ToString());
            Destroy(this.gameObject);
        }
    }
}
