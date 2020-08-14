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

    public override void Activate(GameObject player) 
    {
        Vector2 offset;
        if (Input.GetButtonDown("Use2"))
        {
            offset.x = 1.2f;
        }
        else
        {
            offset.x = -1.2f;
        }
        Vector2 newPos = new Vector2(player.GetComponent<PlayerMovement>().rb.position.x + offset.x, player.GetComponent<PlayerMovement>().rb.position.y - 0.5f);

        GameObject clone = GameObject.Find("Earth Cube(Clone)");
        if (clone != null)
        {
            Destroy(clone);
        }
        GameObject instance = Instantiate(earthCube);
        instance.GetComponent<Rigidbody2D>().transform.SetPositionAndRotation(newPos, new Quaternion());
        instance.transform.SetParent(player.transform);
        //earth cube should have velocity set to the same as the players so that it will path with them if they are in the air.
        SelfDestruct();
    }

    
}
