using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Element
{
    public Color earthColor;             //Color the player will change to
    public Sprite earthSprite;
    public GameObject earthCube;
    public GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        color = earthColor;
        sprite = earthSprite;
    }

    public override void Activate(PlayerMovement player) 
    {
        float offset;
        if (Input.GetButtonDown("Use2"))
        {
            offset = player.rb.GetComponent<SpriteRenderer>().sprite.rect.width / 8;
        }
        else
        {
            offset = -1 * player.rb.GetComponent<SpriteRenderer>().sprite.rect.width / 8;
        }
        Vector2 newPos = new Vector2(player.rb.position.x + offset, player.rb.position.y);
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = Instantiate(earthCube);
        instance.GetComponent<Rigidbody2D>().transform.SetPositionAndRotation(newPos, new Quaternion());
        instance.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
        //earth cube should have velocity set to the same as the players so that it will path with them if they are in the air.
    }
}
