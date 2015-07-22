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

	// Sets up the navmeshagent
	void Start()
	{
		if (!GetComponent<NavMeshAgent>())
		{
			Debug.Log ("NavMeshAgent on " + name + "was not found.\nA temporary one has been added.");
			gameObject.AddComponent<NavMeshAgent>();
		}

		nav = GetComponent<NavMeshAgent>();
	}

	// Runs main mechanic functions
	void Update()
	{
		// If our path has ended, check for a new path
		if (nav.pathStatus == NavMeshPathStatus.PathComplete)
			Movement ();

		Rotation ();

		timeSinceAttack += Time.deltaTime;

		if (timeSinceAttack >= timeBetweenAttacks)
			Attack ();
	}

	// Sets a new path to move
	protected override void Movement()
	{
		if (target != transform && GameController.GetCount<PlayerBehaviour>() > 0)
		{
			PlayerBehaviour[] players = GameObject.FindObjectsOfType<PlayerBehaviour>();

			for (int i = 0; i < players.Length; i++)
			{
				if (Physics.Linecast (transform.position, players[i].transform.position))
				{
					target = players[i].transform;
					break;
				}
				else
					target = this.transform;
			}

			nav.SetDestination(target.position);
		}

		if (!Physics.Linecast (transform.position, target.position) &&
		    nav.pathStatus == NavMeshPathStatus.PathComplete)
		{
			target = this.transform;
			GetNewPos ();
		}

		if (target = this.transform)
			GetNewPos();
	}

	// Just looks forward
	protected override void Rotation()
	{
		transform.LookAt (nav.nextPosition);
	}

	// Does the attack
	protected override void Attack()
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
			nav.SetDestination(GameController.GetRandomPos(transform.position));
		} while (nav.pathStatus == NavMeshPathStatus.PathInvalid);
	}
}