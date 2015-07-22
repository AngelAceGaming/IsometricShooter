using UnityEngine;
using System.Collections;

public class PlayerBehaviour : CharacterBehaviour 
{
	// The vectors for movement and rotation
	Vector3 mov;
	Vector3 rot;

	// The character controller for movement
	CharacterController c;

	// Sets the character controller up
	protected void Start()
	{
		if (Camera.main.GetComponent<CameraController>())
			Camera.main.GetComponent<CameraController>().target = this.transform;
		else
			Debug.Log ("Main camera in this scene needs a CameraController attached");

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

		Debug.Log ((c.isGrounded) ? "Grounded" : "Not grounded");

		// Checks if we should attack
		if (Input.GetKeyDown (GameController.attack))
			Attack();
	}

	// The movement
	protected override void Movement()
	{
		// Sets which direction we should move
		mov.x = Input.GetAxis ("Vertical");
		mov.z = -Input.GetAxis ("Horizontal");

		/* Just normalizes the direction
		 * This stops the object moving faster
		 * When it is going diagonally
		 */
		mov.x = mov.normalized.x;
		mov.z = mov.normalized.z;

		// Applies the speed to our movement
		mov.x *= speed;
		mov.z *= speed;

		if (!c.isGrounded)
			mov.y -= GameController.gravity * Time.deltaTime;
		else
			mov.y = 0f;

		// If we're grounded and press jump
		if (c.isGrounded && Input.GetKeyDown (GameController.jump))
		{
			Debug.Log ("Adding force");
			mov.y = jumpForce;
		}
		
		// Applies movement
		c.Move(mov * Time.deltaTime);
	}

	// Rotates the player towards the mouse cursor
	protected override void Rotation()
	{
		// Gets the angle between the object and the mouse
		float angle = AngleBetweenPositions(Camera.main.WorldToViewportPoint(transform.position),
		                                    Camera.main.ScreenToViewportPoint(Input.mousePosition));

		/* Just sets our rotation along the Y axis
		 * This just avoids it from rotating on its side
		 */
		rot = new Vector3 (0f, angle, 0f);

		// Sets the rotation
		transform.rotation = Quaternion.Euler(rot);
	}

	/* Works out the angle from the game object to the mouse cursor
	 * You might need to change this if you're coming in from a different angle
	 */
	float AngleBetweenPositions(Vector3 a, Vector3 b)
	{
		/* Gets the arc tangent of the object position and mouse position
		 * Converts it to radians
		 * Takes 45 degrees off the result so it faces the mouse
		 */
		return -Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg - 45f;
	}

	// Does attack behaviour
	protected override void Attack()
	{
		Debug.Log ("ATTACK!");

		// A raycasthit to check if we hit anything
		RaycastHit hit;

		// Shoots a ray forward from our shoot point
		if (Physics.Raycast(shootFrom.position, transform.forward, out hit))
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