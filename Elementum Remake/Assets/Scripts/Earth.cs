using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Element
{
    public Color earthColor;             //Color the player will change to
    public Sprite earthSprite;
    public GameObject earthCube;

    // Start is called before the first frame update
    void Start()
    {
        color = earthColor;
        sprite = earthSprite;
        name = "Earth";
    }

    public override void Activate(GameObject player, Vector2 direction) 
    {
        Vector2 offset;
        if (direction.x > 0)
        {
            offset.x = 1f;
        }
        else
        {
            offset.x = -1f;
        }
        Vector2 newPos = new Vector2(player.transform.position.x + offset.x, player.transform.position.y - 0.2f);

        GameObject clone = GameObject.Find("Earth Cube(Clone)");
        if (clone != null)
        {
            Destroy(clone);
        }
        GameObject instance = Instantiate(earthCube);
        instance.GetComponent<Rigidbody2D>().transform.SetPositionAndRotation(newPos, new Quaternion());
        Vector2 playerVelocity = player.GetComponent<Rigidbody2D>().velocity;
        instance.GetComponent<Rigidbody2D>().velocity = playerVelocity;
        //earth cube should have velocity set to the same as the players so that it will path with them if they are in the air.
        SelfDestruct();
    }

    
}
