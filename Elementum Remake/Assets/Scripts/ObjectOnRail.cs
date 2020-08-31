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
    public Rigidbody2D moveableObject;
    public MovingPlatform platform;
    public float speed;
    public float limit;
    public float turnCooldown;
    public AudioSource movingEarth;

    public Vector2 vel;

    public MovementType mode;

    // Start is called before the first frame update
    void Start()
    {
        limit = GetComponent<SpriteRenderer>().size.x / 2;
        platform = moveableObject.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (mode)
        {
            case MovementType.SimpleX:
                SimpleTravelX();
                break;
            case MovementType.RubberbandY:
                RubberbandY();
                break;
            case MovementType.RubberbandX:
                RubberbandX();
                break;
        }
    }

    private void SimpleTravelX()
    {
        if (active)
        {
            if (moveableObject.position.x > transform.position.x + limit || moveableObject.position.x < transform.position.x - limit)
            {
                speed *= -1;
            }
            moveableObject.MovePosition(new Vector2(moveableObject.position.x + speed * Time.deltaTime, transform.position.y));
            platform.velocityOffset = new Vector2(speed, 0);
            if (!movingEarth.isPlaying)
            {
                movingEarth.Play();
            }
        }
        else
        {
            movingEarth.Stop();
            platform.velocityOffset = Vector2.zero;
        }
    }

    private void RubberbandX()
    {
        if (!movingEarth.isPlaying)
        {
            movingEarth.Play();
        }
        if (active)
        {
            if (moveableObject.position.x <= transform.position.x - limit)
            {
                movingEarth.Stop();
                platform.velocityOffset = Vector2.zero;
            }
            else
            {
                moveableObject.MovePosition(new Vector2(moveableObject.transform.position.x - speed * Time.deltaTime, moveableObject.position.y));

            }
        }
        else
        {
            if (moveableObject.position.x >= transform.position.x + limit)
            {
                movingEarth.Stop();
                platform.velocityOffset = Vector2.zero;
            }
            else
            {
                platform.velocityOffset = new Vector2(speed, 0);
                moveableObject.MovePosition(new Vector2(moveableObject.position.x + speed * Time.deltaTime, moveableObject.position.y));

            }
        }
    }

    private void RubberbandY()
    {
        if (!movingEarth.isPlaying)
        {
            movingEarth.Play();
        }
        if (active)
        {
            
            if (moveableObject.position.y <= transform.position.y - limit)
            {
                movingEarth.Stop();
                platform.velocityOffset = Vector2.zero;
            }
            else
            {
                platform.velocityOffset = new Vector2(0, - speed);
                moveableObject.MovePosition(new Vector2(moveableObject.transform.position.x, moveableObject.position.y - speed * Time.deltaTime));

            }
        }
        else
        {
            if (moveableObject.position.y >= transform.position.y + limit)
            {
                movingEarth.Stop();
                platform.velocityOffset = Vector2.zero;
            }
            else
            {
                platform.velocityOffset = new Vector2(0, speed);
                moveableObject.MovePosition(new Vector2(moveableObject.position.x, moveableObject.position.y + speed * Time.deltaTime));

            }
        }
        
    }
}