using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;
    public SpriteRenderer render;
    public SpriteRenderer capeRender;

    public GameObject ghost;
    public float ghostDelay;
    private float ghostDelayTimer;
    private float ghostDuration;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.playerTurned += Turn;
        Air.airCast += AirAbility;
        Fire.fireCast += FireAbility;
        ghostDelayTimer = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimationParameters();

        ghostDelayTimer -= Time.deltaTime;
        ghostDuration -= Time.deltaTime;
        if (ghostDelayTimer < 0 && ghostDuration > 0)
        {
            //generate ghost
            CreateGhost();
        }
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
        anim.SetBool("Landed", player.landed);
        anim.SetBool("Pushing", player.pushing);
        anim.SetFloat("xInput", player.movement.inputVector.x);
    }

    public void Die()
    {
        anim.SetTrigger("Dead");
    }

    public void AirAbility()
    {
        anim.SetTrigger("AirAbility");
        ghostDuration = 0.5f;
    }

    public void FireAbility()
    {
        ghostDuration = 0.5f;
    }

    public void Turn(bool isTurningLeft)
    {
        if (render.flipX == !isTurningLeft)
        {
            anim.SetTrigger("Turn");
        }
        GetComponent<SpriteRenderer>().flipX = isTurningLeft;
    }

    public void CreateGhost()
    {
        GameObject currentghost = Instantiate(ghost, transform.position, transform.rotation);
        SpriteRenderer ghostRender = currentghost.GetComponent<SpriteRenderer>();
        SpriteRenderer ghostCapeRender = currentghost.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ghostRender.flipX = render.flipX;
        ghostRender.sprite = render.sprite;
        ghostCapeRender.sprite = capeRender.sprite;
        ghostCapeRender.flipX = capeRender.flipX;
        ghostCapeRender.color = capeRender.color;
        ghostDelayTimer = ghostDelay;
    }
}
