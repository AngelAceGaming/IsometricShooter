using UnityEngine;
using System.Collections;

public class PlayerBehaviour : CharacterBehaviour 
{
	public Transform shootPoint;

	Vector3 mov;
	Vector3 rot;

	protected override void Movement()
	{
		mov.x = Input.GetAxis ("Horizontal");
		mov.y = Input.GetAxis ("Vertical");
	}

	protected override void Rotation()
	{
		rot = Input.mousePosition;
		rot.z = transform.position.z;
		transform.LookAt (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	protected override void Attack()
	{
		RaycastHit hit;

		if (Physics.Raycast(shootPoint.position, transform.forward, out hit))
		{
			if (hit.collider.GetComponent<CharacterBehaviour>())
				hit.collider.GetComponent<CharacterBehaviour>().LoseHP(damage);
		}
	}

	protected override void Dead()
	{
		gameObject.SetActive(false);
	}
}