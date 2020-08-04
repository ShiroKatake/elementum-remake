using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : Element
{
    public Color airColor;              //Color the player will change to
    public Sprite airSprite;
    public float forceMultiplier;       //Alter the force of the players standard jump

    public AudioClip airSound;

    // Start is called before the first frame update
    void Start()
    {
        color = airColor;
        sprite = airSprite;
        name = "Air";
    }

    public override void Activate(PlayerMovement player)
    {
        SoundManager.PlaySound(airSound);
        player.Jump(Vector2.up, player.jumpForce * forceMultiplier);
    }
}
