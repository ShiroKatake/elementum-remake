using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

	public PlayerController player;
	

	[Header("Animation")]
	public Animator anim;

	public float ladderTimer;
	public bool climbing;

	[Header("Stats")]
	public float speed;					//How fast the player moves
	public float wallJumpLerp;          //How much movement is restricted during wall jump
	public float fallMultiplier;
	public float lowJumpMultiplier;

	[Header("Checks")]
	public bool Immobilized;            //Flag triggered by dash to prevent player movement during ability
	public Vector2 previousVelocity;
	public bool falling;
	public bool turning;

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
		//Take movement input from player
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (player.alive)
		{
			if (Immobilized || player.jump.wallCoyoteTime || player.disabled || new Vector2(x, y) == new Vector2(0, 0))
			{
				anim.SetBool("Moving", false);
			}
			else
			{
				Walk(new Vector2(x, y));
			}
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

		if ((lastX > 0 && x < lastX) || (lastX < 0 && x > lastX))
		{
			turning = true;
		}
		else
		{
			turning = false;
		}

		if (Immobilized || player.ability.active)
		{
		}
		else
		{
			BetterFall();
		}

		if (player.Position == Position.Air)
		{
			if (rb.velocity.y < 0)
			{
				falling = true;
				anim.SetBool("Falling", true);
			}
		}
		else
		{
			falling = false;
			anim.SetBool("Falling", false);
		}

		ladderTimer -= Time.deltaTime;
		lastX = x;
	}

	public void BetterFall()
	{
		if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
		else if (rb.velocity.y < 0 )
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
	}

	public void LateUpdate()
	{
		if (rb.velocity.y != 0 && rb.velocity.x != 0)
		{
			previousVelocity = rb.velocity;
		}
	}

	void Walk(Vector2 dir) 
	{
			
		anim.SetBool("Moving", true);
		if (!player.jump.wallCoyoteTime)
		{
			if (dir.x < 0)
			{
				if (!GetComponent<SpriteRenderer>().flipX)
				{
					anim.SetTrigger("Turn");
				}
				GetComponent<SpriteRenderer>().flipX = true;
			}
			else
			{
				if (GetComponent<SpriteRenderer>().flipX)
				{
					anim.SetTrigger("Turn");
				}
				GetComponent<SpriteRenderer>().flipX = false;
			}
		}
		if (player.Position == Position.Ground) 
		{
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} 
		else 
		{
			//If wall jumping, lerping the input will act as a damp so the player won't regain control immediately and accidentally cancel the wall jump
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
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
