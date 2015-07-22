using UnityEngine;
using System.Collections;

public class BasicPlayer : PlayerBehaviour 
{
	public Transform gun;

	public ParticleSystem[] shells;
	public ParticleSystem[] muzzleFlashes;

	public int amount;

	int index;
	
	#pragma warning disable 108
	void Start()
	{
		base.Start();

		ParticleSystem shellPrefab = shells[0];
		ParticleSystem muzzlePrefab = muzzleFlashes[0];

		shells = new ParticleSystem[amount];
		muzzleFlashes = new ParticleSystem[amount];

		if (amount >= 1)
		{
			for (int i = 0; i < amount; i++)
			{
				ParticleSystem shell = Instantiate (shellPrefab) as ParticleSystem;
				ParticleSystem muzzle = Instantiate (muzzlePrefab) as ParticleSystem;

				if (shell == null)
					Debug.LogError ("No shell.");
				if (muzzle == null)
					Debug.LogError ("No muzzle.");
				
				shell.transform.SetParent(gun);
				muzzle.transform.SetParent(gun);
			
				shell.transform.localPosition = shellPrefab.transform.localPosition;
				muzzle.transform.localPosition = muzzlePrefab.transform.localPosition;

				shells[i] = shell;
				muzzleFlashes[i] = muzzle;
			}
		}

		index = 0;
	}
	#pragma warning restore 108

	protected override void Attack()
	{
		if (shells[index] != null)
			shells[index].Play();
		else
			Debug.Log ("Shell error");

		if (muzzleFlashes[index] != null)
			muzzleFlashes[index].Play ();
		else
			Debug.Log ("Muzzle error");

		index = (index + 1 < amount) ? index + 1 : 0;

		base.Attack ();
	}
}