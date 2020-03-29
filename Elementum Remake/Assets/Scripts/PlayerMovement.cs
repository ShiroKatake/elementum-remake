using System.Collections;
using UnityEngine;

public enum CollisionSide
{
    Left,
    Right,
    None
}

public enum PlayerPosition
{
	Wall,
	Ground,
	Air
}

public class PlayerMovement : MonoBehaviour {
	[Header("Stats")]
	public float jumpForce;             //How high the player jumps
	public float speed = 10f;           //How fast the player moves
	public float wallJumpLerp;          //How much movement is restricted during wall jump
	public float dashForce;				//How powerful the dash pushes the player
	public float dashTime;				//How long the player hangs in the air

	[Header("Checks")]
	public bool canMove = true;
	public bool wallSlide;
	public bool wallJumped;
    public bool airJump;                //Flag triggered when the jump method is called from the air ability

    public GameObject earthCube;
	public AbilitySlot slot;

	private Collision coll;             //Player's collision box
	private Rigidbody2D rb;             //Player's rigidbody
	private float initialGravity;       //Initial gravity value on player's rigidbody
    private CollisionSide wallSide;     //Tracks side that colliding wall is relation to the player

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collision>();
	}

	private void Start() {
		initialGravity = rb.gravityScale;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        Vector3 contactPoint = collision.contacts[0].point;
        if (contactPoint.x > rb.transform.position.x + rb.GetComponent<SpriteRenderer>().sprite.rect.width / 2)
        {
            wallSide = CollisionSide.Right;
        }
        else if (contactPoint.x < rb.transform.position.x + rb.GetComponent<SpriteRenderer>().sprite.rect.width / 2)
        {
            wallSide = CollisionSide.Left;
        }
        else
        {
            wallSide = CollisionSide.None;
        }
    }

	private void Update() {
		//Take movement input from player
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		Walk(new Vector2(x, y));

		//Wall slide check
        if (coll.onWall || coll.onGround)
        {
            airJump = false;
        }
		if (coll.onWall && !coll.onGround) {
			wallSlide = true;
		}
		if (!coll.onWall || coll.onGround) {
			wallSlide = false;
		}
		if (coll.onGround) {
			wallJumped = false;
		}

        //When colliding with a wall, reduce player's y velocity to simulate the wall slide effect
        if (wallSlide) {
            canMove = false;
            if ((wallSide == CollisionSide.Right && Input.GetButtonDown("Horizontal")) || (wallSide == CollisionSide.Left && !Input.GetButtonDown("Horizontal")))
            {
                canMove = true;
            }
			if (rb.velocity.y <= 0) {
				rb.velocity *= new Vector2(1, 0.3f);
			}
		} else {
			canMove = true;
		}


		if (Input.GetButtonDown("Jump")) {
			if (coll.onGround)
				Jump(Vector2.up, jumpForce);
			if (wallSlide)
				WallJump();
		}

		if (Input.GetButtonDown("Use") || Input.GetButtonDown("Use2") && slot.occupied)
		{
			slot.Activate(this);
			slot.occupied = false;
			airJump = true;
		}
	}

	void Walk(Vector2 dir) {
		if (!canMove)
			return;
		if (!wallJumped) {
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} else {
			//If wall jumping, lerping the input will act as a damp so the player won't regain control immediately and accidentally cancel the wall jump
			Debug.Log("lerping");
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
		}
	}

	public void Jump(Vector2 dir, float force) {
		rb.velocity = new Vector2(rb.velocity.x, 0);	//Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better. Setting only y velocity to 0 allows for air controls
		rb.velocity += dir * force;
	}

	private void WallJump() {
		wallSlide = false;
		Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;	//Work out which direction to wall jump to
		Jump(Vector2.up + wallDir, jumpForce * 0.7f);
		wallJumped = true;
	}

	public void Dash(Vector2 dir, float dashForce) {
		StartCoroutine(DisableMovement());
		wallJumped = false;								//Cancel the momentum from wall jump if the player is wall jumping
		rb.velocity -= rb.velocity;						//Resetting velocity to 0 allows for instant response to the player's input -> Makes it feel better
		rb.velocity += dir * dashForce;
	}

	//Disabling input and change gravity to 0 before applying the new velocity will set the player up to move horizontally across
	IEnumerator DisableMovement() {
		canMove = false;
		rb.gravityScale = 0;							//Also disabling BetterJump because the script deals with gravity
		GetComponent<BetterJump>().enabled = false;

		yield return new WaitForSeconds(dashTime);

		canMove = true;
		rb.gravityScale = initialGravity;
		GetComponent<BetterJump>().enabled = true;
	}
}
