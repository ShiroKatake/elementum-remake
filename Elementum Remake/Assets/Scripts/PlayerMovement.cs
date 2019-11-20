﻿using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float jumpForce;				//Player's jump force
	public float speed = 10f;           //Player's speed
	public float wallJumpLerp;			//Determines how much movement is restricted during wall jump

	private Rigidbody2D rb;             //Player's rigidbody
	private Collision coll;				//Player's collision box
	private Vector2 move;               //Player's movement
	private float initialGravity;       //Initial gravity value

	private bool wallJumped;            //
	private bool isDashing;
	private bool wallGrab;
	private bool canMove = true;

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
		Walk(new Vector2(x, y));

		//Wall grab
		if (coll.onWall && !coll.onGround) {
			wallGrab = true;
		}

		//if (wallGrab) {
			//canMove = false;
			//rb.velocity = Vector2.zero;
			//rb.gravityScale = 0;
		//} else {
		//	rb.gravityScale = initialGravity;
		//}

		if (Input.GetButtonDown("Jump")) {
			if (coll.onGround || wallGrab)
            {
                Jump(Vector2.up, false, jumpForce);
            }
				
			if (wallGrab)
            {
                WallJump();
            }
				
		}

		if(coll.onGround && !isDashing) {
			wallJumped = false;
			GetComponent<BetterJump>().enabled = true;
		}
		
        if(Input.GetButtonDown("Use") && GetComponent<AbilitySlot>().occupied)
        {
            switch (GetComponent<AbilitySlot>().element.name)
            {
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
			rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
		}
	}

	void Jump(Vector2 dir, bool wall, float force) {
		//Allow for air controls
		rb.velocity = new Vector2(rb.velocity.x, 0);
		//Jump
		rb.velocity += dir * force;
	}

	void WallJump() {;
		Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;
		Jump(Vector2.up / 1.5f + wallDir / 1.5f, true, jumpForce);
		wallJumped = true;
        wallGrab = false;
    }

	IEnumerator DisableMovement(float time) {
        Debug.Log("Movement Disabled");
		
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}
}
