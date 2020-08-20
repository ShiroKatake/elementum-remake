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

public enum Action
{
    MoutingEarth,
    OnLadder,
    Jumping,
    Falling,
    Walking
}

//This Script is responsible for keeping track of the player's state and allowing other scripts to access some variables from one another
public class PlayerController : MonoBehaviour
{
    private static bool spawned = false;
    public static PlayerController player;

    public delegate void simpleDelegate();
    public static event simpleDelegate deathEvent;
    public static event simpleDelegate playerFalling;


    [Header("Player Script References")]
    public PlayerMovement movement;
    public PlayerJump jump;
    public Collision coll;
    public PlayerSound sound;
    public PlayerAbility ability;
    public PlayerAnimation animations;
    public PlayerWallInteractions wall;

    public SpriteRenderer render;
    public GameObject cape;

    [Header("")]
    public Color capeDefault;
    public GameObject airPuff;
    public GameObject dustPuff;
    public GameObject wallDustPuff;

    [Header("Debugging")]
    public bool debugSpawn;
    public TMP_Text debugCoyoteTime;
    public bool debug;

    [Header("Position")]
    public bool onLadder;
    public Position previousPosition;
    public Position playerPosition;
    public Action playerAction;
    public bool mountingEarth;
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
        SceneController.ScenePhaseChanged += ChangePlayerState;
        
    }

    //Each time the scene phase changes, the player will be notified and will enable and disable stripts in accordance
    public void ChangePlayerState(ScenePhase current, ScenePhase next)
    {
        switch(next)
        {
            
            case ScenePhase.Loading:
            case ScenePhase.Open:
            case ScenePhase.Paused:
            case ScenePhase.Close:
                player.movement.disabled = true;
                player.jump.disabled = true;
                player.ability.disabled = true;
                break;
            case ScenePhase.Cinematic:
                player.jump.disabled = true;
                player.ability.disabled = true;
                break;
            case ScenePhase.Game:
                player.movement.disabled = false;
                player.jump.disabled = false;
                player.ability.disabled = false;
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
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
        if (mountingEarth && jump.wallJumped)
        {
            mountingEarth = false;
        }

        //set the Enum playerPosition to correspond with what the player is colliding with
        SetPosition();
        //set the Enum playerAction to correspond with what the player is currently doing
        SetAction();

        if (debug)
        {
            debugCoyoteTime.text = playerPosition.ToString();
        }

        if (ability.queue.Empty)
        {
            cape.GetComponent<SpriteRenderer>().color = capeDefault;
        }
        else
        {
            cape.GetComponent<SpriteRenderer>().color = ability.queue.queue[ability.queue.LastOccupiedSlot].color;
        }

        

        if (Input.GetButtonDown("Respawn"))
        {
            Respawn();
        }

        //if (playerPosition == Position.WallLeft)
        //{
        //    render.flipX = true;
        //}
        //else if (playerPosition == Position.WallRight)
        //{
        //    render.flipX = false;
        //}
        //if (playerPosition == Position.Air)
        //{
        //    if (previousPosition == Position.WallLeft)
        //    {
        //        render.flipX = false;
        //    }
        //    else if (previousPosition == Position.WallRight)
        //    {
        //        Debug.Log("flipping");
        //        render.flipX = true;
        //    }
        //}
        

        cape.GetComponent<SpriteRenderer>().flipX = render.flipX;
    }

    public bool MountingEarthInAir()
    {
        if (player.previousPosition == Position.Air && player.mountingEarth)
        {
            return true;
        }
        return false;
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
            animations.Die();
            alive = false;
            sound.DeathSound();
        }
    }

    public bool OnWall()
    {
        if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight)
        {
            return true;
        }
        return false;
    }

    public void AirPuff()
    {
        GameObject puff = Instantiate(airPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

    public void DustPuff()
    {
        if (playerPosition == Position.Ground)
        {
        GameObject puff = Instantiate(dustPuff);
        puff.transform.position = new Vector2(transform.position.x, transform.position.y);

        }
    }

    public void WallDustPuff()
    {
        GameObject puff = Instantiate(wallDustPuff);
        float xOffset = 0;
        if (render.flipX)
        {
            xOffset = -0.3f;
        }
        else
        {
            puff.GetComponent<SpriteRenderer>().flipX = true;
            xOffset = 0.3f;
        }
        puff.transform.position = new Vector2(transform.position.x+xOffset, transform.position.y-1);
        
    }

    public void SetPosition()
    {
        previousPosition = playerPosition;
        if (coll.onGround)
        {
            playerPosition = Position.Ground;
        }
        if (coll.onLeftWall && previousPosition != Position.Ground)
        {
            playerPosition = Position.WallLeft;
        }
        if (coll.onRightWall && previousPosition != Position.Ground)
        {
            playerPosition = Position.WallRight;
        }
        if (!coll.onGround && !coll.onWall)
        {
            playerPosition = Position.Air;
        }

    }

    public void SetAction()
    {
        if (movement.moving)
        {
            playerAction = Action.Walking;
        }
        if (jump.jumped)
        {
            playerAction = Action.Jumping;
        }
        if (onLadder)
        {
            playerAction = Action.OnLadder;
        }
        if (movement.falling)
        {
            playerAction = Action.Falling;
            playerFalling?.Invoke();
        }
        if (MountingEarthInAir())
        {
            playerAction = Action.MoutingEarth;
        }
    }
}
