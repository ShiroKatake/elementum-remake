﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Element
{
    public Color earthColor;             //Color the player will change to

    // Start is called before the first frame update
    void Start()
    {
        color = earthColor;
    }

    public override void Activate(PlayerMovement player) 
    {
        //float offset;
        //                  if (Input.GetButtonDown("Use2"))
        //                  {
        //                      offset = rb.GetComponent<SpriteRenderer>().sprite.rect.width / 8;
        //                  }
        //                  else
        //                  {
        //                      offset = -1 * rb.GetComponent<SpriteRenderer>().sprite.rect.width / 8;
        //                  }
        //                  Vector2 newPos = new Vector2(rb.position.x + offset, rb.position.y);
        //                  earthCube.GetComponent<Rigidbody2D>().transform.SetPositionAndRotation(newPos, new Quaternion());
        //                  //earth cube should have velocity set to the same as the players so that it will path with them if they are in the air.
        //			break;
    }
}
