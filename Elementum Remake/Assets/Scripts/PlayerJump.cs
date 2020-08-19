using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool disabled;

    //coupled variables
    public Animator anim;
    public PlayerController player;
    public AudioClip playerJump;

    [Header("Jump Modifiers")]
    public bool coyoteTime;
    public bool coyoteTimeAvailable;
    public bool jumped;
    public float queuedjump;
    public float jumpForce = 22;				//How high the player jumps
    public float wallJumpForceModifier;

    [Header("Wall Modifiers")]
    public bool wallCoyoteTime;
    public bool wallJumped;
    
    public bool wallSlide;
    public bool wallGrab;
    public float slideMultiplier;
    public float slideTimer;
    public bool mountingEarth;

    public Position inputBlocked;
    public float inputBlockTimer;

    // Start is called before the first frame update
    void Start()
    {
        slideMultiplier = 0.001f;
        anim = GetComponent<PlayerMovement>().anim;
        player.movement.rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            jumped = false;
            wallJumped = false;
        }
        
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (!jumped && player.PreviousPosition == Position.Ground)
            {
                StartCoroutine(CoyoteTime());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        inputBlockTimer -= Time.deltaTime;
        if (inputBlockTimer < 0)
        {
            inputBlocked = Position.Air;
        }

        player.movement.rb.gravityScale = 5;

        //Check the script is not disabled by the playercontroller 
        if (!disabled)
        {
            if (mountingEarth && wallJumped)
            {
                mountingEarth = false;
            }
            //WallStuff
            if (player.Position == Position.WallLeft || player.Position == Position.WallRight)
            {
                if (player.movement.falling)
                {
                    wallSlide = true;
                    wallGrab = false;

                    //Turn the player based on what wall they are on
                    if (player.Position == Position.WallLeft)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else if (player.Position == Position.WallRight)
                    {

                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                    

                    //Player will stop moving if they are holding the wall, but will slide and fall off if not
                    if (HoldingWall(player.Position))
                    {
                        wallGrab = true;
                        slideMultiplier = 0;
                        player.movement.rb.gravityScale = 0;
                        slideTimer = 1;
                    }
                    else if (LeavingWall(player.Position))
                    {
                        StartCoroutine(WallCoyoteTime());
                    }
                    else
                    {
                        player.movement.rb.velocity = new Vector2(0, player.movement.rb.velocity.y);
                        slideTimer -= Time.deltaTime;
                        if (slideTimer > 0)
                        {
                            slideMultiplier += 0.05f;
                            slideTimer = 1;
                        }
                        slideMultiplier = 0.7f;
                    }
                    
                    
                    player.movement.rb.velocity *= new Vector2(1, slideMultiplier);
                }
            }
            else
            {
                wallSlide = false;
                slideMultiplier = 0.001f;
            }

            if (player.alive)
            {
                //JumpStuff
                if (Input.GetButtonDown("Jump"))
                {
                    queuedjump = 0.2f;
                    if (player.Position == Position.Ground || coyoteTime)
                    {
                        SoundManager.PlaySound(playerJump);
                        Jump(Vector2.up, jumpForce, player.movement.rb.velocity.x);
                    }
                    else if (player.Position == Position.WallLeft || player.Position == Position.WallRight)
                    {
                        wallJumped = true;
                        SoundManager.PlaySound(playerJump);
                        WallJump();
                    }
                }

                if ((queuedjump > 0))
                {
                    if (player.Position == Position.Ground)
                    {
                        SoundManager.PlaySound(playerJump);
                        Jump(Vector2.up, jumpForce, player.movement.rb.velocity.x);
                    }
                    else if (player.Position == Position.WallLeft || player.Position == Position.WallRight)
                    {
                        wallJumped = true;
                        SoundManager.PlaySound(playerJump);
                        WallJump();

                    }
                }
            }
        }

        queuedjump -= Time.deltaTime;
    }

    public bool HoldingWall(Position wall)
    {
        if (wall == Position.WallLeft && Input.GetAxis("Horizontal") < 0)
        {
            return true;
        }
        else if (wall == Position.WallRight && Input.GetAxis("Horizontal") > 0)
        {
            return true;
        }
        return false;
    }

    public bool LeavingWall(Position wall) 
    {
        if (wall == Position.WallLeft && Input.GetAxis("Horizontal") > 0)
        {
            return true;
        }
        else if (wall == Position.WallRight && Input.GetAxis("Horizontal") < 0)
        {
            return true;
        }
        return false;
    }

    public bool MountingEarthInAir()
    {
        if (player.previousPosition == Position.Air && mountingEarth)
        {
            return true;
        }
        return false;
    }

    public void Jump(Vector2 dir, float force, float x)
    {
        player.playerPosition = Position.Air;
        jumped = true;
        player.movement.rb.velocity = new Vector2(x, 0);    //Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better. Setting only y velocity to 0 allows for air controls
        player.movement.rb.velocity += dir * force;
        queuedjump = 0;
    }

    private void WallJump()
    {
        wallCoyoteTime = false;
        inputBlocked = player.Position;
        Vector2 wallDir = (player.Position == Position.WallRight) ? Vector2.left : Vector2.right;    //Work out which direction to wall jump to
        Jump(Vector2.up + wallDir, jumpForce * wallJumpForceModifier, player.movement.rb.velocity.x);
        inputBlockTimer = 0.1f;
        
    }

    private IEnumerator CoyoteTime()
    {
        coyoteTimeAvailable = false;
        coyoteTime = true;
        yield return new WaitForSeconds(0.1f);
        coyoteTime = false;
    }

    public void Freeze()
    {
        player.movement.rb.velocity = new Vector2(0, 0);
    }

    //Players tend to move away as they jump off a wall to maximise distance. This little pause give the player some time to jump after they press the direction key.
    private IEnumerator WallCoyoteTime()
    {
        wallCoyoteTime = true;
        yield return new WaitForSeconds(0.2f);
        wallCoyoteTime = false;

    }
}
