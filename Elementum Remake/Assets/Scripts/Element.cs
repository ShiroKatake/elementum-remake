using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    protected Color color;

    public Sprite sprite;


    public Color Color
    {
        get
        {
            return color;
        }
    }

    public abstract void Activate(PlayerMovement player);
}
