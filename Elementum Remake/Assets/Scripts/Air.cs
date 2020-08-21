using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : Element
{
    public delegate void ElementDelegate();
    public static event ElementDelegate airCast;

    public Color airColor;              //Color the player will change to
    public Sprite airSprite;
    

    

    // Start is called before the first frame update
    void Start()
    {
        color = airColor;
        sprite = airSprite;
        name = "Air";
    }

    public override void Activate(GameObject player, Vector2 direction)
    {
        airCast?.Invoke();
        SelfDestruct();
    }
}
