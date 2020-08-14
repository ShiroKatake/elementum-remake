using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CollisionSide
{
    Left,
    Right,
    None
}

public enum Position
{
    WallLeft,
    WallRight,
    Ground,
    Ladder,
    Air
}

public class PlayerController : MonoBehaviour
{
    private static bool spawned = false;
    public static PlayerController player;

    public delegate void DeathDelegate();
    public static event DeathDelegate deathEvent;

    public PlayerMovement movement;
    public PlayerJump jump;
    public Collision coll;
    public PlayerSound sound;
    public PlayerAbility ability;
    public Animator anim;

    public GameObject airPuff;
    public GameObject dustPuff;

    [Header("Debugging")]
    public bool debugSpawn;
    public TMP_Text debugCoyoteTime;
    public bool debug;

    [Header("Position")]
    public bool onLadder;
    public Position previousPosition;
    public Position playerPosition;

    public GameObject holding;

    public bool cinematicOverride;
    public bool disabled;
    public bool alive;


    public Position Position => playerPosition;

    public Position PreviousPosition => previousPosition;

    private void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);

            player = this;

            if (debugSpawn)
            {
                GameData.spawnLocation = GameObject.Find("TestSpawnPoint").transform.position;
            }
            Respawn();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        debugCoyoteTime = GameObject.Find("/Player Camera/Debug/PlayerPosition").GetComponent<TMP_Text>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (holding != null)
        {
            if (collision.gameObject == holding.GetComponent<Key>().door)
            {
                holding.GetComponent<Key>().Activate();
            }
        }
        if (collision.gameObject.tag == "Ladder")
        {
            onLadder = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            onLadder = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //set the Enum playerPosition to correspond with what the player is colliding with
        SetPosition();

        if (debug)
        {
            debugCoyoteTime.text = playerPosition .ToString();
        }

        if (holding != null)
        {
            holding.transform.position = new Vector2(transform.position.x, transform.position.y + 1.3f);
        }

        if (Input.GetButtonDown("Respawn"))
        {
            Respawn();
        }

        //reset the airjump to false when the player is no longer in the air
        if (player.Position != Position.Air)
        {
            if (player.PreviousPosition == Position.Air)
            {
                if (!onLadder)
                {
                    sound.LandSound();
                }
                ability.active = false;
            }
        }
        SetAnimationParameters();
    }

    private void SetAnimationParameters()
    {
        anim.SetBool("OnLadder", movement.climbing);
        anim.SetBool("WallGrab", jump.wallSlide);
        anim.SetBool("WallCoyote", jump.wallCoyoteTime);
        anim.SetBool("Jumping", jump.jumped);
        anim.SetBool("Falling", movement.falling);
        anim.SetBool("SlowingDown", movement.turning);
        anim.SetBool("AirTurning", movement.turning);
    }

    public void Respawn()
    {
        Debug.Log("respawning");
        transform.position = new Vector3(GameData.spawnLocation.x, GameData.spawnLocation.y);
        alive = true;
    }

    public void Die()
    {
        deathEvent?.Invoke();
        if (alive)
        {
            Debug.Log("Dead");
            anim.SetTrigger("Dead");
            alive = false;
            sound.DeathSound();
            disabled = true;
        }
    }

    public void AirPuff()
    {
        GameObject puff = Instantiate(airPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

    public void DustPuff()
    {
        GameObject puff = Instantiate(dustPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void SetPosition()
    {
        previousPosition = playerPosition;
        if (coll.onLeftWall && !coll.onGround)
        {
            playerPosition = Position.WallLeft;
        }
        if (coll.onRightWall && !coll.onGround)
        {
            playerPosition = Position.WallRight;
        }
        if (coll.onGround)
        {
            playerPosition = Position.Ground;
        }
        if (!coll.onGround && !coll.onWall)
        {
            playerPosition = Position.Air;
        }

    }
}
