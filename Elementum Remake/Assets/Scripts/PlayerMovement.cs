using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[Header("Stats")]
	public float jumpForce;             //Player's jump force
	public float speed = 10f;           //Player's speed
	public float wallJumpLerp;          //Determines how much movement is restricted during wall jump

	[Header("Checks")]
	public bool canMove = true;
	public bool wallTouch;
	public bool wallGrab;
	public bool wallJumped;

	private Collision coll;             //Player's collision box
	private Rigidbody2D rb;             //Player's rigidbody
	private Vector2 move;               //Player's movement
	private float initialGravity;       //Initial gravity value

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collision>();
	}

	private void Start() {
		initialGravity = rb.gravityScale;
	}

	private void Update() {
		//Take movement input from player
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		//Wall grab check
		if (coll.onWall && !coll.onGround)
			wallGrab = true;
		if (!coll.onWall || coll.onGround)
			wallGrab = false;

		//Simulate the effect of wall grab by making the player's gravity 0 and have them stay still
		if (wallGrab) {
			canMove = false;
			rb.gravityScale = 0;
		} else {
			canMove = true;
			rb.gravityScale = initialGravity;
		}

		//Reset player's velocity when touching the wall (to avoid having momentum when gravity = 0)
		if (coll.onWall && !wallTouch && !coll.onGround) {
			wallTouch = true;
			rb.velocity -= rb.velocity;
		}
		if (!coll.onWall && wallTouch) {
			wallTouch = false;
		}

		Walk(new Vector2(x, y));

		if (Input.GetButtonDown("Jump")) {
			if (coll.onGround || wallGrab)
				Jump(Vector2.up, false, jumpForce);
			if (wallGrab)
				WallJump();
		}

		if (Input.GetButtonDown("Use") && GetComponent<AbilitySlot>().occupied) {
			switch (GetComponent<AbilitySlot>().element.name) {
				case "Air":
					Jump(Vector2.up, false, jumpForce * 1.5f);
					break;
				case "Fire":
					break;
				case "Earth":
					break;
			}
			GetComponent<AbilitySlot>().occupied = false;
		}
	}

	void Walk(Vector2 dir) {
		if (!canMove)
			return;
		if (!wallJumped) {
			rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
		} else {			
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime); //Lerping the input will act as a damp so that the player won't regain control immediately and cancel the jump accidentally
		}
	}

	void Jump(Vector2 dir, bool wall, float force) {
		rb.velocity = new Vector2(rb.velocity.x, 0);	//Setting only y component to 0 allows for air controls
		rb.velocity += dir * force;
	}

	void WallJump() {
		wallGrab = false;		
		Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;	//Work out which direction to jump to
		Jump(Vector2.up / 1.5f + wallDir / 1.5f, true, jumpForce);
		wallJumped = true;
	}

	//This will be useful for dash later
	IEnumerator DisableMovement(float time) {
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}
}
