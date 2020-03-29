using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : Element
{
    public Color airColor;

    // Start is called before the first frame update
    void Start()
    {
        color = airColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Activate(PlayerMovement player)
    {
        player.Jump(Vector2.up, player.jumpForce * 1.3f);
    }
}
