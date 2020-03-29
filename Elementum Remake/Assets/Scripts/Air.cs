using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : Element
{
    public Color airColor;              //Color the player will change to
    public float forceMultiplier;       //Alter the force of the players standard jump

    // Start is called before the first frame update
    void Start()
    {
        color = airColor;
    }

    public override void Activate(PlayerMovement player)
    {
        player.Jump(Vector2.up, player.jumpForce * forceMultiplier);
    }
}
