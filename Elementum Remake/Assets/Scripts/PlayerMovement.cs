﻿using System.Collections;
using UnityEngine;

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
	Air
}

public class PlayerMovement : MonoBehaviour 
{
	[Header("Stats")]
	public float jumpForce;             //How high the player jumps
	public float speed = 10f;           //How fast the player moves
	public float wallJumpLerp;          //How much movement is restricted during wall jump

	[Header("Jump Modifiers")]
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public bool coyoteTime;
	public bool coyoteTimeAvailable;
	public bool jumped;
	public float queuedjump;

	[Header("Checks")]
	public bool Immobilized;            //Flag triggered by dash to prevent player movement during ability


	public bool wallCoyoteTime;
	public bool wallSlide;
	public bool wallJumped;
	public bool earthJumped;
	public bool mountingEarth;
    public bool airJump;                //Flag triggered when the jump method is called from the air ability
	public Position playerPosition;

	[Header("Abilities")]
	public AbilityQueue queue;

	[Header("Physics")]
	private Collision coll;             //Player's collision box
	public Rigidbody2D rb;             //Player's rigidbody
	public float initialGravity;       //Initial gravity value on player's rigidbody
	public Vector3 spawnPoint;

	private void Awake() 
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collision>();
		//Debug.Log(GameData.spawnLocation.x + " : " + GameData.spawnLocation.y);
		rb.transform.position = new Vector3(GameData.spawnLocation.x, GameData.spawnLocation.y);
	}

	private void Start() 
	{
		initialGravity = rb.gravityScale;
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		
		if (collision.gameObject.tag == "Earth")
		{
			if (wallSlide)
			{
				mountingEarth = true;
			}
		}

		if (collision.gameObject.tag == "Ladder") 
		{
			if (Input.GetAxis("Vertical") > 0)
			{
				Debug.Log("laffering");
				rb.velocity = new Vector2(rb.velocity.x/2, 10);
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 8)
		{
			wallJumped = false;
			jumped = false;
		}
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 8)
		{
			if (!jumped && playerPosition == Position.Air)
			{
				StartCoroutine(CoyoteTime());
			}
		}
	}

	private void Update() 
	{
		//Take movement input from player
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		Walk(new Vector2(x, y));

		//set the Enum playerPosition to correspond with what the player is colliding with
		SetPosition();

		if (!Immobilized)
		{
			BetterJump();
		}
		
		//player can't accidentally use air ability on ground
        if (playerPosition != Position.Air)
        {
			airJump = false;
			
		}
		if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight) 
		{
			if ((playerPosition == Position.WallRight && Input.GetButtonDown("Horizontal")) || (playerPosition == Position.WallLeft && !Input.GetButtonDown("Horizontal")))
			{
				StartCoroutine(WallCoyoteTime());
			}
			if (rb.velocity.y <= 0)
			{
				wallSlide = true;
				rb.velocity *= new Vector2(1, 0.3f);
			}
		}
		else
		{
			wallSlide = false;
			
		}

		if ((queuedjump > 0))
		{
			if (playerPosition == Position.Ground)
			{
				Jump(Vector2.up, jumpForce);
			}
			else if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight)
			{
				wallJumped = true;
				WallJump();

			}
		}

		queuedjump -= Time.deltaTime;
		if (Input.GetButtonDown("Jump")) 
		{
			queuedjump = 0.2f;
			if (playerPosition == Position.Ground || coyoteTime)
			{
				Jump(Vector2.up, jumpForce);
			}
			else if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight)
			{
				wallJumped = true;
				WallJump();
				
			}
		}

		if (Input.GetButtonDown("Use") || Input.GetButtonDown("Use2"))
		{
			queue.Activate(this);
			airJump = true;
		}
	}

	void Walk(Vector2 dir) 
	{
		if (Immobilized || wallCoyoteTime)
		{
			return;
		}
		if (playerPosition == Position.Ground) 
		{
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} 
		else 
		{
			//If wall jumping, lerping the input will act as a damp so the player won't regain control immediately and accidentally cancel the wall jump
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
		}
	}

	public void BetterJump()
	{
		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if ((rb.velocity.y > 0 && !Input.GetButton("Jump")) || rb.GetComponent<PlayerMovement>().airJump)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	public void Jump(Vector2 dir, float force) 
	{
		jumped = true;
		rb.velocity = new Vector2(rb.velocity.x, 0);	//Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better. Setting only y velocity to 0 allows for air controls
		rb.velocity += dir * force;
		queuedjump = 0;
	}

	private void WallJump() {
		Freeze();
		Vector2 wallDir = (playerPosition == Position.WallRight) ? Vector2.left : Vector2.right;	//Work out which direction to wall jump to
		Jump(Vector2.up + wallDir, jumpForce * 0.7f);
		queuedjump = 0;
	}

	public void Freeze()
	{
		rb.velocity = new Vector2(0, 0);
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

	private IEnumerator WallCoyoteTime()
	{
		wallCoyoteTime = true;
		yield return new WaitForSeconds(0.2f);
		wallCoyoteTime = false;

	}

	//Players tend to move away as they jump off a wall to maximise distance. This little pause give the player some time to jump after they press the direction key.
	private IEnumerator CoyoteTime()
	{
		coyoteTimeAvailable = false;
		coyoteTime = true;
		yield return new WaitForSeconds(0.2f);
		coyoteTime = false;
	}

	public void SetPosition()
	{
		if (coll.onLeftWall && !coll.onGround)
		{
			playerPosition = Position.WallLeft;
		}
		if (coll.onRightWall && !coll.onGround)
		{
			playerPosition = Position.WallRight;
		}
		if (coll.onGround)
		{
			playerPosition = Position.Ground;
		}
		if (!coll.onGround && !coll.onWall)
		{
			playerPosition = Position.Air;
		}

	}
}
