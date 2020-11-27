using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element
{
    public delegate void ElementDelegate();
    public static event ElementDelegate fireCast;
    
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
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        //Stop player from moving and execute dash
        movement.DisableMovement(true);
        fireCast?.Invoke();
        StartCoroutine(Dash(dir, dashForce, movement));
        
    }

    private IEnumerator Dash(Vector2 dir, float dashForce, PlayerMovement movement)
    {
        movement.Dash(dir, dashForce);
        yield return new WaitForSeconds(dashTime);
        movement.DisableMovement(false);
        SelfDestruct();
    }
}
