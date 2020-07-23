using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element
{
    public Color fireColor;             //Color the player will change to
    public Sprite fireSprite;
    public float dashForce;             //How powerful the dash pushes the player
    public float dashTime;

    // Start is called before the first frame update
    void Start()
    {
        color = fireColor;
        sprite = fireSprite;
        name = "Fire";
    }
    public override void Activate(PlayerMovement player)
    {
        // Determine the direction of the dash
        Vector2 dir;
        if (Input.GetButtonDown("Use2"))
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
            
        }

        //Stop player from moving and execute dash
        StartCoroutine(player.DisableMovement(dashTime));
        StartCoroutine(Dash(dir, dashForce, player));
    }

    private IEnumerator Dash(Vector2 dir, float dashForce, PlayerMovement player)
    {
        player.rb.velocity -= player.rb.velocity;           //Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better
        player.rb.velocity = dir * dashForce;

        yield return new WaitForSeconds(dashTime);

        
    }
}
