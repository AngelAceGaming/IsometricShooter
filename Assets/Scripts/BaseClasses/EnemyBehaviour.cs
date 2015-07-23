using UnityEngine;
using System.Collections;

public class EnemyBehaviour : CharacterBehaviour 
{
	// How far in front we should set an overlap sphere to attack
	public float offset;

	// The size of the overlap sphere
	public float attackSize;
	
	// How much time needs to pass betwe
	public float timeBetweenAttacks;

	// This handles our pathfinding
	NavMeshAgent nav;
	
	// How much time has passed since we last attacked
	float timeSinceAttack;

	// The current object being chased
	Transform target;

#if UNITY_EDITOR
	public bool debug;
	public Color attackColor = Color.white;
#endif

	// Sets up the navmeshagent
	void Start()
	{
		if (!GetComponent<NavMeshAgent>())
		{
			Debug.Log ("NavMeshAgent on " + name + "was not found.\nA temporary one has been added.");
			gameObject.AddComponent<NavMeshAgent>();
		}

		nav = GetComponent<NavMeshAgent>();

		target = this.transform;

		//Movement ();
	}

	// Runs main mechanic functions
	void Update()
	{
		// Do our movement
		Movement ();

		// Do our rotation
		Rotation ();

		// Updates our attack timer
		timeSinceAttack += Time.deltaTime;

		//If our attack timer greater or equal to our timer, attack
		if (timeSinceAttack >= timeBetweenAttacks)
			Attack ();
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// Allows you to see how big you're setting your attack size to
		if (debug)
		{
			Gizmos.color = attackColor;
			Gizmos.DrawSphere(transform.position + transform.forward * offset, attackSize);
		}
	}
#endif

	// Sets a new path to move
	protected override void Movement()
	{
		if (GameController.GetCount<PlayerBehaviour>() > 0)
		{
			PlayerBehaviour[] players = GameObject.FindObjectsOfType<PlayerBehaviour>();

			for (int i = 0; i < players.Length; i++)
			{
				RaycastHit hit;
				if (Physics.Linecast (transform.position, players[i].transform.position, out hit))
				{
					Debug.Log ("Colliding with " + hit.collider.name);
					if (hit.collider.GetComponent<PlayerBehaviour>() &&
					    hit.collider.GetComponent<PlayerBehaviour>() == players[i])
					{
						Debug.DrawLine (transform.position, players[i].transform.position);
						target = players[i].transform;
						Debug.Log (Vector3.Angle(target.position, transform.position));
						break;
					}
					else
						target = this.transform;
				}
			}

			if (target != this.transform)
				nav.SetDestination(target.position);
		}

		if (!Physics.Linecast (transform.position, target.position))
		{
			target = this.transform;
			GetNewPos ();
		}
		else if (Mathf.Abs (Vector3.Distance (transform.position, nav.destination)) < 0.1f)
			GetNewPos ();
	}

	// Just looks forward
	protected override void Rotation()
	{
		transform.LookAt (nav.nextPosition);
	}

	// Does the attack
	protected override void Attack()
	{
		// Only attacks if target is in range and isn't itself
		if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) < offset && target != this.transform)
		{
			// Resets the attack timer
			timeSinceAttack = 0f;

			// Gets an array of the objects to check against
			Collider[] c =	Physics.OverlapSphere(transform.position + transform.forward * offset, attackSize);

			// Runs through the array
			for (int i = 0; i < c.Length; i++)
			{
				/* Checks to make sure the current collider is not this object
				 * If it isn't, check if it has a CharacterBehaviour attached.
				 * If it does, attack it
				 */
				if (c[i].gameObject != gameObject && c[i].GetComponent<CharacterBehaviour>())
					c[i].GetComponent<CharacterBehaviour>().LoseHP(damage);
			}
		}
	}

	// How this object should die
	protected override void Dead()
	{
		gameObject.SetActive(false);
	}

	// Gets a new random position on the map
	void GetNewPos()
	{
		do
		{
			Debug.Log ("finding new path");
			nav.SetDestination(GameController.GetRandomPos(transform.position));
		} while (nav.pathStatus == NavMeshPathStatus.PathInvalid);
	}
}