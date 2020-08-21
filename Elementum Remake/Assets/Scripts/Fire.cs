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
    public override void Activate(GameObject player, Vector2 dir)
    {

        //Stop player from moving and execute dash
        StartCoroutine(player.GetComponent<PlayerMovement>().DisableMovement(dashTime));
        StartCoroutine(Dash(dir, dashForce, player.GetComponent<PlayerMovement>()));
        SelfDestruct();
    }

    private IEnumerator Dash(Vector2 dir, float dashForce, PlayerMovement player)
    {
        player.rb.velocity -= player.rb.velocity;           //Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better
        player.rb.velocity = dir * dashForce;

        yield return new WaitForSeconds(dashTime);

        
    }
}
