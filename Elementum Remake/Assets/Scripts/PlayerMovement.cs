using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	public delegate void turningDelegate(bool isTurningLeft);
	public static event turningDelegate playerTurned;

	public PlayerController player;
	public bool disabled;

	[Header("Animation")]
	public Animator anim;

	public bool onLadder;
	public float ladderTimer;
	public bool climbing;
	public Vector2 climbingVelocity;

	[Header("Stats")]
	public float speed;					//How fast the player moves
	public float wallJumpLerp;          //How much movement is restricted during wall jump
	public float airbourneLerp;
	public float fallMultiplier;
	public float lowJumpMultiplier;

	[Header("Checks")]
	public bool Immobilized;            //Flag triggered by dash to prevent player movement during ability
	public bool falling;
	public bool turning;
	public bool moving;
	public bool JumpButtonDown;

	[Header("Physics")]

	public Vector2 inputVector;
	public BoxCollider2D edgeDetect;
	public Rigidbody2D rb;              //Player's rigidbody
	public float initialGravity;        //Initial gravity value on player's rigidbody
	public float lastX;

	private void Start() 
	{
		player = GetComponent<PlayerController>();
		rb = GetComponent<Rigidbody2D>();
		initialGravity = rb.gravityScale;
		
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ladder"))
		{
			if (!onLadder)
			{
				if (player.Position == Position.Air && inputVector.y > 0.5 && falling)
				{
					Debug.Log("On ladder");
					onLadder = true;
					player.jump.jumped = false;
				}
			}
			
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ladder"))
		{
			onLadder = false;
		}
	}

	private void Update() 
	{
		//Check the script is not disabled by the playercontroller
		if (!disabled)
		{
			if (player.alive)
			{
				//Make sure player movement isnt being prevented by another action
				if (CantMove(inputVector))
				{

				}
				else
				{
					Walk(inputVector);
				}

				//Set the animation to idle if the player is moving a tiny bit or not at all
				if (inputVector.x < 0.1 && inputVector.x > -0.1)
				{
					moving = false;
				}

				//Climbing ladders
				if (onLadder)
				{
					LadderMovement(inputVector.x, inputVector.y);

				}
				else
				{
					climbing = false;
					//rb.gravityScale = 5;
				}
			}

			//Detect if the player is accelerating towards 0
			if ((lastX > 0 && inputVector.x < lastX) || (lastX < 0 && inputVector.x > lastX))
			{
				turning = true;
			}
			else
			{
				turning = false;
			}


			if (Immobilized)
			{
				rb.velocity = new Vector2(rb.velocity.x, 0);
			}

			//Skip the falling acceleration if the player is dashing or airjumping 
			//NOTE: Immobilized might be redundant due to player.ability.active
			if (Immobilized || player.ability.active)
			{

			}
			else
			{
				BetterFall();
			}

			//Check if the player is falling
			if (rb.velocity.y < 0 && player.Position != Position.Ground)
			{
				falling = true;
			}
			else
			{
				falling = false;
			}

			//Save the x position for use in the next frame
			lastX = inputVector.x;
		}

		//count down till the next time the ladder will make a sound
		//NOTE: this can all be replaced with an event on the ladder animation, but will require an idle ladder animation
		ladderTimer -= Time.deltaTime;
	}

	public void ChangeJumpButtonDown(bool change)
	{
		JumpButtonDown = change;
	}

	//Increases gravity on the player if they are not holding the jump button 
	//Or if they are travelling downwards
	public void BetterFall()
	{
		if (!falling && !JumpButtonDown)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
		else if (falling)
		{
			
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
	}


	//Check if any variables that would prevent movement are true
	public bool CantMove(Vector2 move)
	{
		if (Immobilized || player.mountingEarthInAir)
		{
			return true;
		}
		
		return false;
	}

	void Walk(Vector2 dir) 
	{
		moving = true;
		
		//Invoke an event if the player is turning
		if (!player.OnWall())
		{
			if (dir.x < -0.1)
			{
				playerTurned?.Invoke(true);
			}
			else if (dir.x > 0.1)
			{
				playerTurned?.Invoke(false);
			}
		}
		
		//Decide the effect of moving based on whether the player in the air or not
		if (player.OnWall() || (player.Position == Position.Air && player.jump.wallJumped))
		{
			//If wall jumping, lerping the input will act as a damp so the player won't regain control immediately and accidentally cancel the wall jump
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
		}
		else if (player.Position == Position.Air)
		{
			if (inputVector.x > 0.1 || inputVector.x < -0.1)
			{
				rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), airbourneLerp * Time.deltaTime);

			}
		}
		else
		{
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} 
		
	}

	public void LadderMovement(float x, float y)
	{
		if (player.jump.jumped)
		{
			onLadder = false;
			return;
		}
		rb.gravityScale = 0;
		climbingVelocity = Vector2.zero;
		climbing = true;

		if (x > 0.2 || x < -0.2)
		{
			climbingVelocity.x = 5*inputVector.x;
		}
		if (y > 0.2)
		{
			climbingVelocity.y = 7;
			ladderTimer = player.sound.LadderSound(ladderTimer);
		}
		else if (y < -0.2)
		{
			climbingVelocity.y = -7;
			ladderTimer = player.sound.LadderSound(ladderTimer);
		}
		rb.velocity = climbingVelocity;
	}
	
	public void Dash(Vector2 dir, float dashForce)
	{
		rb.velocity = Vector2.zero;
		rb.velocity = dir * dashForce;
		if (dir.x > 0)
		{
			player.render.flipX = false;
		}
		else
		{
			player.render.flipX = true;
		}
	}


	//Disabling input and change gravity to 0 before applying the new velocity will set the player up to move horizontally across
	public void DisableMovement(bool active) 
	{
		Immobilized = active;
		if (active)
		{
			anim.SetTrigger("FireAbility");
			rb.gravityScale = 0;

		}
		else {
			rb.gravityScale = initialGravity;
	
		}
	}
}
