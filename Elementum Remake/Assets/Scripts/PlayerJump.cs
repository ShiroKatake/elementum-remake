using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool disabled;

    //coupled variables
    public PlayerController player;
    public AudioClip playerJump;
    public AudioClip airSound;

    [Header("Jump Checks")]
    public bool coyoteTime;
    public bool coyoteTimeAvailable;
    public bool jumped;
    public bool wallJumped;

    [Header("Jump Modifiers")]
    public float queuedjump;
    public float jumpForce = 22;				//How high the player jumps
    public float wallJumpForceModifier;
    public float airJumpForceMultiplier;       //Alter the force of the players standard jump


    // Start is called before the first frame update
    void Start()
    {
        player.movement.rb = GetComponent<Rigidbody2D>();
        Air.airCast += AirJump;
        
    }

    //Reset checks when the player touches a wall or ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            jumped = false;
            wallJumped = false;
        }
        
    }

    //Grant coyote time if player leaves the ground without jumping
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
        if ((queuedjump > 0))
        {
            if (player.Position == Position.Ground)
            {
                Jump(Vector2.up, jumpForce, player.movement.rb.velocity.x, playerJump);
            }
            else if (player.OnWall())
            {
                WallJump();

            }
        }
        queuedjump -= Time.deltaTime;

    }

    public void Activate()
    {
        //Check the script is not disabled by the playercontroller 
        if (!disabled)
        {
            //Jump
            queuedjump = 0.2f;
            if (player.Position == Position.Ground || coyoteTime)
            {
                Jump(Vector2.up, jumpForce, player.movement.rb.velocity.x, playerJump);
            }
            else if (player.OnWall())
            {
                WallJump();
            }
        }
    }

    
    public void AirJump()
    {
        Jump(Vector2.up, jumpForce * airJumpForceMultiplier, 0, airSound);
    }
    

    public void Jump(Vector2 dir, float force, float x, AudioClip sound)
    {
        //make sure the player can't jump twice for polish sake
        if (!jumped || x == 0)
        {
            SoundManager.PlaySound(sound);
            player.playerPosition = Position.Air;
            jumped = true;
            player.movement.rb.velocity = new Vector2(x, 0);    //Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better. Setting only y velocity to 0 allows for air controls
            player.movement.rb.velocity += dir * force;
            queuedjump = 0;
        }
    }

    private void WallJump()
    {
        wallJumped = true;
        player.wall.coyoteTime = false;
        Vector2 wallDir = (player.Position == Position.WallRight) ? Vector2.left : Vector2.right;    //Work out which direction to wall jump to
        Jump(Vector2.up + wallDir, jumpForce * wallJumpForceModifier, player.movement.rb.velocity.x, playerJump);
        
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
    
}
