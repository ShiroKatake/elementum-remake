using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //coupled variables
    public Animator anim;
    public Rigidbody2D rb;
    public PlayerController player;
    public AudioClip playerJump;

    [Header("Jump Modifiers")]
    public bool coyoteTime;
    public bool coyoteTimeAvailable;
    public bool jumped;
    public float queuedjump;
    public float jumpForce = 22;				//How high the player jumps

    [Header("Wall Modifiers")]
    public bool wallCoyoteTime;
    public bool wallJumped;
    public bool wallSlide;
    public float slideMultiplier;
    public bool mountingEarth;

    // Start is called before the first frame update
    void Start()
    {
        slideMultiplier = 0.3f;
        anim = GetComponent<PlayerMovement>().anim;
        rb = GetComponent<Rigidbody2D>();
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
        if (mountingEarth && wallJumped)
        {
            mountingEarth = false;
        }
        //WallStuff
        if (player.Position == Position.WallLeft || player.Position == Position.WallRight)
        {
            if ((player.Position == Position.WallRight && Input.GetAxis("Horizontal") < 0) || (player.Position == Position.WallLeft && Input.GetAxis("Horizontal") > 0))
            {
                StartCoroutine(WallCoyoteTime());
            }
            if (rb.velocity.y <= 0)
            {
                wallSlide = true;
                if (player.Position == Position.WallLeft && Input.GetAxis("Horizontal") < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (player.Position == Position.WallRight && Input.GetAxis("Horizontal") > 0)
                {

                    GetComponent<SpriteRenderer>().flipX = false;
                }
                if (slideMultiplier < 1)
                {
                    slideMultiplier += 0.003f;
                }
                rb.velocity *= new Vector2(1, slideMultiplier);
            }
        }
        else
        {
            wallSlide = false;
            slideMultiplier = 0.3f;
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
                    Jump(Vector2.up, jumpForce);
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
                    Jump(Vector2.up, jumpForce);
                }
                else if (player.Position == Position.WallLeft || player.Position == Position.WallRight)
                {
                    wallJumped = true;
                    SoundManager.PlaySound(playerJump);
                    WallJump();

                }
            }
        }

        queuedjump -= Time.deltaTime;
    }

    public bool MountingEarthInAir()
    {
        if (player.previousPosition == Position.Air && mountingEarth)
        {
            return true;
        }
        return false;
    }

    public void Jump(Vector2 dir, float force)
    {
        jumped = true;
        rb.velocity = new Vector2(0, 0);    //Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better. Setting only y velocity to 0 allows for air controls
        rb.velocity += dir * force;
        queuedjump = 0;
    }

    private void WallJump()
    {
        Freeze();
        Vector2 wallDir = (player.Position == Position.WallRight) ? Vector2.left : Vector2.right;    //Work out which direction to wall jump to
        Jump(Vector2.up + wallDir, jumpForce * 0.7f);
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
        rb.velocity = new Vector2(0, 0);
    }

    //Players tend to move away as they jump off a wall to maximise distance. This little pause give the player some time to jump after they press the direction key.
    private IEnumerator WallCoyoteTime()
    {
        wallCoyoteTime = true;
        yield return new WaitForSeconds(0.2f);
        wallCoyoteTime = false;

    }
}
