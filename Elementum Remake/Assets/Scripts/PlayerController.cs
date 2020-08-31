using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public PlayerControls input;

    public delegate void simpleDelegate();
    public static event simpleDelegate deathEvent;
    public static event simpleDelegate playerFalling;
    public static event simpleDelegate playerInteract;


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
    public GameObject firePuff;

    [Header("Debugging")]
    public bool debugSpawn;
    public TMP_Text debugCoyoteTime;
    public bool debug;

    [Header("Position")]
    public bool onLadder;
    public Position previousPosition;
    public Position playerPosition;
    public Action playerAction;
    public bool holdingJump;
    public bool mountingEarthInAir;
    public bool alive;
    public bool landed;


    public Position Position => playerPosition;

    public Position PreviousPosition => previousPosition;

    private void Awake()
    {
        if (!spawned)
        {
            spawned = true;
            DontDestroyOnLoad(gameObject);

            input = new PlayerControls();

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
        input.Gameplay.Jump.performed += ctx => jump.Activate();
        input.Gameplay.Jump.started += ctx => movement.ChangeJumpButtonDown(true);
        input.Gameplay.Jump.canceled += ctx => movement.ChangeJumpButtonDown(false);
        input.Gameplay.AbilityLeft.performed += ctx => ability.Activate(new Vector2(-1, 0));
        input.Gameplay.AbilityRight.performed += ctx => ability.Activate(new Vector2(1, 0));
        input.Gameplay.Interact.performed += ctx => playerInteract?.Invoke();
        input.Gameplay.Respawn.performed += ctx => Respawn();


        input.Gameplay.Move.performed += ctx => movement.inputVector = ctx.ReadValue<Vector2>();
        input.Gameplay.Move.canceled += ctx => movement.inputVector = Vector2.zero;


        debugCoyoteTime = GameObject.Find("/Player Camera/Debug/PlayerPosition").GetComponent<TMP_Text>();
        SceneController.ScenePhaseChanged += ChangePlayerState;
        Hazard.hazardEvent += Die;
    }

    private void OnEnable()
    {
        input.Gameplay.Enable();
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
                movement.disabled = true;
                jump.disabled = true;
                ability.disabled = true;
                break;
            case ScenePhase.Cinematic:
                jump.disabled = true;
                ability.disabled = true;
                break;
            case ScenePhase.Game:
                movement.disabled = false;
                jump.disabled = false;
                ability.disabled = false;
                break;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Ladder")
        {
            if (playerPosition == Position.Air)
            {
                onLadder = true;
            }
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
        if (mountingEarthInAir && jump.wallJumped || playerPosition == Position.Ground)
        {
            mountingEarthInAir = false;
        }

        //set the Enum playerPosition to correspond with what the player is colliding with
        SetPosition();
        //set the Enum playerAction to correspond with what the player is currently doing
        SetAction();

        if (debug)
        {
            debugCoyoteTime.text = mountingEarthInAir.ToString();
        }

        if (ability.queue.Empty)
        {
            cape.GetComponent<SpriteRenderer>().color = capeDefault;
        }
        else
        {
            cape.GetComponent<SpriteRenderer>().color = ability.queue.queue[ability.queue.LastOccupiedSlot].color;
        }

        if (playerPosition == Position.Ground)
        {
            ability.active = false;
        }

        if (playerPosition == Position.Ground && previousPosition == Position.Air)
        {
            landed = true;
            
        }
        else
        {
            landed = false;
        }
        

        cape.GetComponent<SpriteRenderer>().flipX = render.flipX;
    }

    public void Respawn()
    {
        Debug.Log("respawning");
        transform.position = new Vector3(GameData.spawnLocation.x, GameData.spawnLocation.y);
        alive = true;
    }

    public void Die(GameObject called)
    {
        if (gameObject == called)
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

    public void FirePuff()
    {
        GameObject puff = Instantiate(firePuff);
        SpriteRenderer render = puff.GetComponent<SpriteRenderer>();
        if (movement.rb.velocity.x > 0)
        {
            render.flipX = false;
        }
        else
        {
            render.flipX = true;
        }
        puff.transform.position = new Vector2(transform.position.x, transform.position.y);
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
        if (mountingEarthInAir)
        {
            playerAction = Action.MoutingEarth;
        }
    }
}
