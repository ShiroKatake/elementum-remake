using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallInteractions : MonoBehaviour
{
    public PlayerController player;

    [Header("Wall Modifiers")]
    public bool coyoteTime;

    public bool slide;
    public bool grab;
    public bool mounted;
    public float slideSpeed;

    public FixedJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        player.movement.rb.gravityScale = 5;

        //WallStuff
        if (player.OnWall() && player.playerAction != Action.Vaulting)
        {
            if (!player.jump.jumped && !coyoteTime && player.movement.falling)
            {
                mounted = true;
                if (player.movement.falling)
                {
                    slide = true;
                    grab = false;
                }
                //Turn the player based on what wall they are on
                if (player.Position == Position.WallLeft)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {

                    GetComponent<SpriteRenderer>().flipX = false;
                }


                slideSpeed = 0;
                player.movement.rb.gravityScale = 0;

                //Player will stop moving if they are holding the wall, but will slide and fall off if not
                if (HoldingWall(player.Position))
                {

                    grab = true;
                }
                else if (LeavingWall(player.Position))
                {
                    StartCoroutine(WallCoyoteTime());
                }
                else
                {
                    slideSpeed = 5;

                }
                if (!player.mountingEarthInAir)
                {
                    player.movement.rb.velocity = new Vector2(0, -slideSpeed);

                }
            }
        }
        else
        {
            slide = false;
            grab = false;
            slideSpeed = 0;
            coyoteTime = false;
            mounted = false;
        }
        if (player.jump.jumped || LeavingWall(player.Position) || player.Position == Position.Air)
        {
            joint.enabled = false;
        }
    }

    public bool HoldingWall(Position wall)
    {
        if (wall == Position.WallLeft && player.movement.inputVector.x < 0)
        {
            return true;
        }
        else if (wall == Position.WallRight && player.movement.inputVector.x > 0)
        {
            return true;
        }
        return false;
    }

    public bool LeavingWall(Position wall)
    {
        if (wall == Position.WallLeft && player.movement.inputVector.x > 0)
        {
            return true;
        }
        else if (wall == Position.WallRight && player.movement.inputVector.x < 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator WallCoyoteTime()
    {
        yield return new WaitForSeconds(0.2f);
        coyoteTime = true;

    }
}
