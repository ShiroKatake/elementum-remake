using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    SimpleX,
    RubberbandY,
    RubberbandX
}

public class ObjectOnRail : MonoBehaviour
{
    public bool active;
    public bool isVertical;
    public float speed;
    public float limit;
    public MovementType mode;

    // Start is called before the first frame update
    void Start()
    {
        limit = GetComponent<SpriteRenderer>().size.x / 2;
    }
}