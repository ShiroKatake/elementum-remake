using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	public float jumpForce;				//Player's jump force
	public float speed = 10f;			//Player's speed

	private Rigidbody2D rb;             //Player's rigidbody
	private Collision coll;				//Player's collision box
	private Vector2 move;				//Player's movement
	
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collision>();
	}

	private void Update() {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		Walk(new Vector2(x, y));

		if(Input.GetButtonDown("Jump")) {
			if (coll.onGround) {
				Jump();
			}
		}
	}

	void Walk(Vector2 dir) {
		rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
	}

	void Jump() {
		//Allow for air controls
		rb.velocity = new Vector2(rb.velocity.x, 0);
		//Jump
		rb.velocity += Vector2.up * jumpForce;
	}
}
