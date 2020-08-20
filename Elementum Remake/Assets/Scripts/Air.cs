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

    public override void Activate(GameObject player)
    {
        SoundManager.PlaySound(airSound);
        player.GetComponent<PlayerController>().animations.anim.SetTrigger("AirAbility");
        player.GetComponent<PlayerJump>().Jump(Vector2.up, player.GetComponent<PlayerJump>().jumpForce * forceMultiplier, 0, airSound);
        SelfDestruct();
    }
}
