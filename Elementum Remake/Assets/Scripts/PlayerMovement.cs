using System.Collections;
using UnityEngine;
using TMPro;

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
	private static bool spawned = false;
	public static PlayerMovement player;

	public GameObject hand;
	public bool cinematicOverride;

	[Header("Debugging")]
	public bool debugSpawn;
	public TMP_Text debugCoyoteTime;

	[Header("Sound")]
	public AudioClip playerJump;
	public AudioClip playerLand;

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
	public float slideMultiplier;
	public bool earthJumped;
	public bool mountingEarth;
    public bool airJump;                //Flag triggered when the jump method is called from the air ability
	public Position previousPosition;
	public Position playerPosition;
	public Vector2 previousVector;

	[Header("Abilities")]
	public AbilityQueue queue;
	public GameObject holding;

	[Header("Physics")]
	private Collision coll;             //Player's collision box
	public BoxCollider2D edgeDetect;
	public Rigidbody2D rb;             //Player's rigidbody
	public float initialGravity;       //Initial gravity value on player's rigidbody

	private void Awake() 
	{
		if(!spawned)
		{
			spawned = true;
			DontDestroyOnLoad(gameObject);
			player = gameObject.GetComponent<PlayerMovement>();
			rb = GetComponent<Rigidbody2D>();
			coll = GetComponent<Collision>();
			if (debugSpawn)
			{
				GameData.spawnLocation = GameObject.Find("TestSpawnPoint").transform.position;
			}
			Respawn();
		}
		else
		{
			DestroyImmediate(gameObject);
		}

	}

	private void Start() 
	{
		initialGravity = rb.gravityScale;
		debugCoyoteTime = GameObject.Find("/Player Camera/Debug/PlayerPosition").GetComponent<TMP_Text>();
		slideMultiplier = 0.3f;
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
			if (Input.GetButton("Up"))
			{
				rb.velocity = new Vector2(rb.velocity.x / 1.2f, 10);
			}
			else if (Input.GetButton("Down"))
			{
				rb.velocity = new Vector2(rb.velocity.x / 1.2f, -10);
			}
			else
			{
				if (rb.velocity.y < 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, 0);
				}			}
		}
		if (holding != null)
		{
			if (collision.gameObject == holding.GetComponent<Key>().door)
			{
				holding.GetComponent<Key>().Activate();
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
			if (!jumped && previousPosition == Position.Ground)
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

		//reset the hand as false each frame
		hand.SetActive(false);

		debugCoyoteTime.text = playerPosition.ToString();


		if (!Immobilized)
		{
			BetterJump();
		}

		if (holding != null)
		{
			holding.transform.position = new Vector2(transform.position.x, transform.position.y + 1.3f);
		}

		if (Input.GetButtonDown("Respawn"))
		{
			Respawn();
		}

		//reset the airjump to false when the player is no longer in the air
		if (playerPosition != Position.Air)
		{
			airJump = false;

		}

		

		if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight)
		{


			if ((playerPosition == Position.WallRight && Input.GetAxis("Horizontal") < 0) || (playerPosition == Position.WallLeft && Input.GetAxis("Horizontal") > 0))
			{
				Debug.Log("this is happening");
				StartCoroutine(WallCoyoteTime());
			}
			if (rb.velocity.y <= 0)
			{
				if (playerPosition == Position.WallLeft && Input.GetAxis("Horizontal") < 0)
				{
					wallSlide = true;
					hand.SetActive(true);
					hand.GetComponent<SpriteRenderer>().flipX = true;
				}
				else if (playerPosition == Position.WallRight && Input.GetAxis("Horizontal") > 0)
				{
					wallSlide = true;
					hand.SetActive(true);
					hand.GetComponent<SpriteRenderer>().flipX = false;
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

		if (!cinematicOverride)
		{
			if (Input.GetButtonDown("Jump"))
			{
				queuedjump = 0.2f;
				if (playerPosition == Position.Ground || coyoteTime)
				{
					SoundManager.PlaySound(playerJump);
					Jump(Vector2.up, jumpForce);
				}
				else if (playerPosition == Position.WallLeft || playerPosition == Position.WallRight)
				{
					wallJumped = true;
					SoundManager.PlaySound(playerJump);
					WallJump();

				}
			}
		}

		if (Input.GetButtonDown("Use") || Input.GetButtonDown("Use2"))
		{
			queue.Activate(this);
			airJump = true;
		}
	}

	public void Respawn()
	{
		Debug.Log("respawning");
		rb.transform.position = new Vector3(GameData.spawnLocation.x, GameData.spawnLocation.y);
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
		yield return new WaitForSeconds(0.1f);
		coyoteTime = false;
	}

	public void SetPosition()
	{
		previousPosition = playerPosition;
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
