using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour 
{
	// The variables for our HP, max HP, damage and speed
	public float hp, maxHp, damage, speed;

	// Just says we'll add these functions in later classes
	protected abstract void Movement();
	protected abstract void Rotation();
	protected abstract void Attack();
	protected abstract void Dead();

	// Adds HP to current HP, but doesn't let it exceed max HP
	public void GainHP(float amount)
	{
		hp = Mathf.Clamp (hp + amount, 0f, maxHp);
	}

	// Removes HP then checks if character is dead
	public void LoseHP(float amount)
	{
		hp = Mathf.Clamp (hp - amount, 0f, maxHp);
		
		if (hp <= 0f)
			Dead ();
	}
}