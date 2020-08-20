using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;
    public SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.playerTurned += Turn;
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimationParameters();
    }

    private void SetAnimationParameters()
    {
        anim.SetBool("OnLadder", player.movement.climbing);
        anim.SetBool("WallMounted", player.wall.mounted);
        anim.SetBool("WallGrab", player.wall.grab);
        anim.SetBool("Jumping", player.jump.jumped);
        anim.SetBool("Falling", player.movement.falling);
        anim.SetBool("SlowingDown", player.movement.turning);
        anim.SetBool("AirTurning", player.movement.turning);
        anim.SetBool("Moving", player.movement.moving);
    }

    public void Die()
    {
        anim.SetTrigger("Dead");
    }

    public void Turn(bool isTurningLeft)
    {
        if (render.flipX == !isTurningLeft)
        {
            anim.SetTrigger("Turn");
        }
        GetComponent<SpriteRenderer>().flipX = isTurningLeft;
    }
}
