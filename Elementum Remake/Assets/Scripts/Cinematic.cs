using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Time.timeScale = 0;
        }
    }
}
