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

	public float ladderTimer;
	public bool climbing;

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

	[Header("Physics")]
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
	private void Update() 
	{
		//Check the script is not disabled by the playercontroller
		if (!disabled)
		{
			//Take movement input from player
			float x = Input.GetAxis("Horizontal");
			float y = Input.GetAxis("Vertical");

			if (rb.velocity.x < -5 || rb.velocity.x > 5)
			{
				speed = 10;
			}
			else
			{
				speed = 15;
			}

			

			if (player.alive)
			{
				//Make sure player movement isnt being prevented by another action
				if (CantMove(new Vector2(x, y)))
				{

				}
				else
				{
					Walk(new Vector2(x, y));
				}

				//Set the animation to idle if the player is moving a tiny bit or not at all
				if (x < 0.1 && x > -0.1)
				{
					moving = false;
				}

				//Climbing ladders
				if (player.onLadder)
				{
					climbing = true;
					if (Input.GetButton("Up"))
					{
						rb.velocity = new Vector2(rb.velocity.x / 1.2f, 10);
						ladderTimer = player.sound.LadderSound(ladderTimer);
					}
					else if (Input.GetButton("Down"))
					{
						rb.velocity = new Vector2(rb.velocity.x / 1.2f, -10);
						ladderTimer = player.sound.LadderSound(ladderTimer);
					}
					else
					{
						climbing = false;
					}
				}
				else
				{
					climbing = false;
				}
			}

			//Detect if the player is accelerating towards 0
			if ((lastX > 0 && x < lastX) || (lastX < 0 && x > lastX))
			{
				turning = true;
			}
			else
			{
				turning = false;
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
			lastX = x;
		}

		//count down till the next time the ladder will make a sound
		//NOTE: this can all be replaced with an event on the ladder animation, but will require an idle ladder animation
		ladderTimer -= Time.deltaTime;
	}


	//Increases gravity on the player if they are not holding the jump button 
	//Or if they are travelling downwards
	public void BetterFall()
	{
		if (!falling && !Input.GetButton("Jump"))
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
		if (Immobilized || player.MountingEarthInAir())
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
			if (dir.x < 0)
			{
				playerTurned?.Invoke(true);
			}
			else
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
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), airbourneLerp * Time.deltaTime);
		}
		else
		{
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} 
		
	}

	
	


	//Disabling input and change gravity to 0 before applying the new velocity will set the player up to move horizontally across
	public IEnumerator DisableMovement(float dashTime) 
	{
		
		Immobilized = true;                 //Also disabling BetterJump because the script deals with gravity
		rb.gravityScale = 0;

		yield return new WaitForSeconds(dashTime);

		Immobilized = false;
		rb.gravityScale = initialGravity;
	}
}
