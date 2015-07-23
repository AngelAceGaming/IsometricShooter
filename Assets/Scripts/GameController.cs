using UnityEngine;

public static class GameController
{
	public const float gravity = 10f;

	public static KeyCode jump = KeyCode.Space;
	public static KeyCode attack = KeyCode.Mouse0;

	public static int GetCount<T>()
		where T : Object
	{
		return GameObject.FindObjectsOfType<T>().Length;
	}

	public static Vector3 GetRandomPos(Vector3 pos)
	{
		pos.x += Random.Range (-20f, 21f);
		pos.y += Random.Range (-20f, 21f);
		pos.z += Random.Range (-20f, 21f);

		return pos;
	}
	
	/* Works out the angle from the game object to the mouse cursor
	 * You might need to change this if you're coming in from a different angle
	 */
	public static float AngleBetweenPositions(Vector3 a, Vector3 b)
	{
		/* Gets the arc tangent of the object position and mouse position
		 * Converts it to radians
		 * Takes 45 degrees off the result so it faces the mouse
		 */
		return -Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg - 45f;
	}
}