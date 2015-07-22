using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public Transform target;

	// How far the camera should be from the target
	public float distance = 3f;
	// How high above the player we want to target
	public float height = 5f;
	// How smoothly the camera should move
	public float damping = 5f;

	void LateUpdate()
	{
		if (target != null)
		{
			// Gets the local height from the target
			Vector3 wantedPosition = target.TransformPoint(0, height, 0);

			// Sets Gets the position from behind the player to give the correct view
			wantedPosition.x = target.position.x - distance;
			wantedPosition.z = target.position.z - distance;

			// Smoothly transitions from camera position to the wanted position
			transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);

			// Looks at the player
			transform.LookAt (target);
		}
		else // The camera has no target to follow
			Debug.Log ("Not following");
	}
}