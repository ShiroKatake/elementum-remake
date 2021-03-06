﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public delegate void eventDelegate(int id);
    public static event eventDelegate eventTrigger;


    public bool repeatable;
    public bool activated;
    public int id;

    private void Start()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>().input.Gameplay.Interact.phase == UnityEngine.InputSystem.InputActionPhase.Started)
            {
                if (activated && !repeatable)
                {

                }
                else
                {
                    activated = true;
                    eventTrigger.Invoke(id);
                }
            }
        }
    }
}
