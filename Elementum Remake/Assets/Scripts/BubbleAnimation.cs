using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAnimation : MonoBehaviour
{
    public bool animating;
    private int frames = 0;

    // Start is called before the first frame update
    void Start()
    {
        animating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            frames += 1;
            transform.localScale *= 1.1f;
            if (transform.localScale.x >= 1)
            {
                Reset();
            }
        }
    }

    public void Reset()
    {
        Debug.Log("Bubble resetting");
        for (int i = 0; i < frames; i++)
        {
            transform.localScale /= 1.1f;
        }
        frames = 0;
        animating = false;
    }
}
