using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Simple,
    Rubberband
}

public class ObjectOnRail : MonoBehaviour
{
    public bool active;
    public bool isVertical;
    public GameObject moveableObject;
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
    }

    // Update is called once per frame
    void Update()
    {

        if (isVertical)
        {
            vel = new Vector2(0, speed);
        }
        else
        {
            vel = new Vector2(speed, 0);
        }
        switch (mode)
        {
            case MovementType.Simple:
                SimpleTravel();
                break;
            case MovementType.Rubberband:
                RubberbandY();
                break;
        }
    }

    private void SimpleTravel()
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
            moveableObject.GetComponent<Rigidbody2D>().velocity = vel;
        }
        else
        {
            moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    private void RubberbandY()
    {
        if (active)
        {
            if (moveableObject.transform.position.y <= transform.position.y - limit)
            {
                moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                movingEarth.Stop();
                return;
            }
            moveableObject.GetComponent<Rigidbody2D>().velocity = -1 * vel;
            
        }
        else
        {
            if (moveableObject.transform.position.y >= transform.position.y + limit)
            {
                moveableObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                movingEarth.Stop();
                return;
            }
            moveableObject.GetComponent<Rigidbody2D>().velocity = vel;
        }
        if (!movingEarth.isPlaying)
        {
            movingEarth.Play();
        }
    }
}