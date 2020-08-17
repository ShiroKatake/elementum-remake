﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIcon : MonoBehaviour
{
    public SpriteRenderer icon;

    // Start is called before the first frame update
    void Start()
    {
        Interaction.interactEvent += Activate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Activate(bool active)
    {
        if (active)
        {
            icon.color = new Color(255, 255, 255, 255);
        }
        else
        {
            icon.color = new Color(255, 255, 255, 0);
        }
    }
}
