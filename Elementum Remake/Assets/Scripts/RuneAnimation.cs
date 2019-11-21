using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneAnimation : MonoBehaviour
{
    public bool animating;
    public GameObject bubbleAnimation;

    // Start is called before the first frame update
    void Start()
    {
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        transform.GetComponent<SpriteRenderer>().color = tmp;
        animating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            Color tmp = transform.GetComponent<SpriteRenderer>().color;
            tmp.a = 1f;
            transform.GetComponent<SpriteRenderer>().color = tmp;
            if (!bubbleAnimation.GetComponent<BubbleAnimation>().animating)
            {
                Reset();
            }
        }
    }

    public void Reset()
    {
        Debug.Log("Rune Resetting");
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        transform.GetComponent<SpriteRenderer>().color = tmp;
        animating = false;
    }
}
