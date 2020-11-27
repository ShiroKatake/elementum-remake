using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleOnRail : ObjectOnRail
{
    public GameObject moveableObject;

    // Start is called before the first frame update
    void Start()
    {
        limit = GetComponent<SpriteRenderer>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (!(moveableObject.transform.position.y >= transform.position.y + limit))
            {
                moveableObject.transform.position = new Vector2(moveableObject.transform.position.x, moveableObject.transform.position.y + speed);
            }

        }
        else
        {
            if (!(moveableObject.transform.position.y <= transform.position.y - limit))
            {
                moveableObject.transform.position = new Vector2(moveableObject.transform.position.x, moveableObject.transform.position.y - speed);
            }

        }
    }
}
