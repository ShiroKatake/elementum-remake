using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViollController : MonoBehaviour
{

    public GameObject target;
    public SpriteRenderer render;
    

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x > transform.position.x)
        {
            render.flipX = false;
        }
        else
        {
            render.flipX = true;
        }
    }

    
}
