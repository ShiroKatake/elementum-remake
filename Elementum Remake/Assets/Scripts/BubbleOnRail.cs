using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleOnRail : MonoBehaviour
{
    public bool active;
    public bool isVertical;
    public GameObject moveableObject;
    public float speed;
    public float limit;

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
            if (moveableObject.transform.position.x >= transform.position.x + limit || moveableObject.transform.position.x <= transform.position.x - limit)
            {
                speed *= -1;
            }
            moveableObject.transform.position = new Vector2(moveableObject.transform.position.x + speed, moveableObject.transform.position.y);
        }
    }
}
