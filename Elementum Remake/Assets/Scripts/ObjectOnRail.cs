using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnRail : MonoBehaviour
{
    public bool active;
    public bool isVertical;
    public GameObject moveableObject;
    public float speed;
    public float limit;
    public float turnCooldown;

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
            if (turnCooldown <= 0)
            {
                if (moveableObject.transform.position.x >= transform.position.x + limit || moveableObject.transform.position.x <= transform.position.x - limit)
                {
                    moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    speed *= -1;
                    turnCooldown = 0.2f;
                }
            }
            turnCooldown -= Time.deltaTime;
            moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        }
        else
        {
            moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
