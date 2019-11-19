using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	public float jumpForce;				//Player's jump force
	public float speed = 10f;			//Player's speed

	private Rigidbody2D rb;				//The player's rigidbody
	private Vector2 move;				//Player's movement
	
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		Walk(new Vector2(x, y));
	}

	void Walk(Vector2 dir) {
		rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
	}
}
