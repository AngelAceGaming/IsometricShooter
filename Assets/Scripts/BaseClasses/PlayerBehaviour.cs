using UnityEngine;
using System.Collections;

public class PlayerBehaviour : CharacterBehaviour 
{
	// The position byou shoot from
	public Transform shootPoint;

	// The vectors for movement and rotation
	Vector3 mov;
	Vector3 rot;

	// The character controller for movement
	CharacterController c;

	// Sets the character controller up
	void Start()
	{
		if (!GetComponent<CharacterController>())
		{
			Debug.Log ("CharacterController on " + name + "was not found.\nA temporary one has been added");
			gameObject.AddComponent<CharacterController>();
		}

		c = GetComponent<CharacterController>();
	}

	// Calls main mechanic functions
	void Update()
	{
		Movement ();
		Rotation ();

		// Checks if we should attack
		if (Input.GetKeyDown (GameController.attack))
			Attack();
	}

	// The movement
	protected override void Movement()
	{
		// Sets which direction we should move
		mov.x = Input.GetAxis ("Horizontal");
		mov.z = Input.GetAxis ("Vertical");

		// If we're NOT grounded, applies gravity
		if (!c.isGrounded)
		{
			mov.y -= GameController.gravity;
		}
		// Hits here if we ARE grounded. Checks if we should jump
		else if (Input.GetKeyDown (GameController.jump))
			mov.y = GameController.jumpForce;
		/* Hits here if we're grounded and not jumping
		 * Resets gravity
		 */
		else
			mov.y = 0f;

		// Applies movement
		c.Move(transform.position + (mov * speed) * Time.deltaTime);
	}

	// Rotates the player towards the mouse cursor
	protected override void Rotation()
	{
		rot = Input.mousePosition;
		rot.z = transform.position.z;
		transform.LookAt (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	// Does attack behaviour
	protected override void Attack()
	{
		// A raycasthit to check if we hit anything
		RaycastHit hit;

		// Shoots a ray forward from our shoot point
		if (Physics.Raycast(shootPoint.position, transform.forward, out hit))
		{
			// If we hit something with a characterbehaviour on, do damage
			if (hit.collider.GetComponent<CharacterBehaviour>())
				hit.collider.GetComponent<CharacterBehaviour>().LoseHP(damage);
		}
	}

	// Called when HP hits 0
	protected override void Dead()
	{
		gameObject.SetActive(false);
	}
}