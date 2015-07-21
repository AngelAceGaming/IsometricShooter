using UnityEngine;

public static class GameController
{
	public const float gravity = 5f;
	public const float jumpForce = 20f;

	public static KeyCode jump = KeyCode.Space;
	public static KeyCode attack = KeyCode.Mouse0;

	public static int Enemycount<T>()
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
}