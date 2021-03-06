﻿using System.Collections;
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
    Walking,
    Vaulting,
    Idle
}

//This Script is responsible for keeping track of the player's state and allowing other scripts to access some variables from one another
public class PlayerController : MonoBehaviour
{
    private static bool spawned = false;
    public PlayerControls input;

    public delegate void simpleDelegate();
    public static event simpleDelegate deathEvent;
    public static event simpleDelegate playerRespawn;
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
    public Legs legs;

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
    public Position previousPosition;
    public Position playerPosition;
    public Action playerAction;
    public bool holdingJump;
    public bool mountingEarthInAir;
    public bool alive;
    public bool landed;
    public bool pushing;
    public bool vaulting;
    public Vector2 vaultPosition;
    public Vector2 vaultPosA;
    public Vector2 vaultPosB;
    public int ledgeSide;


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


        debugCoyoteTime = GameObject.Find("/Player Camera/HUD/PlayerPosition").GetComponent<TMP_Text>();
        SceneController.ScenePhaseChanged += ChangePlayerState;
        Hazard.hazardEvent += Die;
        ExitInteraction.doorEvent += Freeze;
    }

    private void OnEnable()
    {
        input.Gameplay.Enable();
    }

    public void Freeze(Vector2 position, string scene)
    {
        jump.Freeze();
    }

    //Each time the scene phase changes, the player will be notified and will enable and disable stripts in accordance
    public void ChangePlayerState(ScenePhase current, ScenePhase next)
    {
        switch(next)
        {
            
            case ScenePhase.Loading:
            case ScenePhase.Open:
            case ScenePhase.Paused:
            case ScenePhase.Dialogue:
            case ScenePhase.Close:
            case ScenePhase.Cinematic:
                LockMovement(true);
                break;
            case ScenePhase.Game:
                LockMovement(false);
                break;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Earth"))
        {
            if (collision.contacts[0].point.y >= transform.position.y-0.5)
            {
                pushing = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Earth"))
        {
            pushing = false;
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

        if (playerAction == Action.Vaulting)
        {
            Vault();
        }

        if (legs.touchingGround && playerPosition == Position.Air && playerAction == Action.Falling)
        {
            transform.position += new Vector3(0, 0.1f);
        }

        if (debug)
        {
            debugCoyoteTime.text = SceneController.phase.ToString();
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
            if (previousPosition == Position.Air)
            {
                landed = true;
            }
            else
            {
                landed = false;
            }
        }
        else
        {
            landed = false;
        }
        

        cape.GetComponent<SpriteRenderer>().flipX = render.flipX;
    }

    public void Respawn()
    {
        Debug.Log("respawning player at: " + GameData.spawnLocation);
        playerRespawn?.Invoke();
        transform.position = new Vector3(GameData.spawnLocation.x, GameData.spawnLocation.y);
        animations.Respawn();
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

    public void LockMovement(bool value)
    {
        movement.disabled = value;
        jump.disabled = value;
        ability.disabled = value;
        movement.rb.gravityScale = 0;
        movement.rb.velocity = Vector2.zero;
    }

    public void Vault()
    {
        Debug.Log("vaulting");
        if (!vaulting)
        {
            ledgeSide = -1*coll.wallSide;
            animations.render.flipX = !animations.render.flipX;
            animations.Vault();
            transform.position = new Vector2(transform.position.x + ledgeSide*0.5f, transform.position.y);
            vaultPosition = transform.position;
            LockMovement(true);
            vaulting = true;
        }

        transform.position = vaultPosition;
    }

    public void FinishVault()
    {
        LockMovement(false);
        movement.onLadder = false;
        transform.position = new Vector2(vaultPosition.x + ledgeSide*0.5f, Mathf.Floor(vaultPosition.y + 2)-0.5f);
        vaulting = false;
        movement.rb.gravityScale = 5;
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
        else
        {
            playerAction = Action.Idle;
        }
        if (jump.jumped)
        {
            playerAction = Action.Jumping;
        }
        if (movement.onLadder)
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
        if (coll.onWall && !coll.overLedge)
        {
            playerAction = Action.Vaulting;
        }
        
    }
}
