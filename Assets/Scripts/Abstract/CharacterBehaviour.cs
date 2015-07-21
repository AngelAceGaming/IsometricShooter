using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour 
{
	public float hp, maxHp, damage, speed;
	
	protected abstract void Movement();
	protected abstract void Rotation();
	protected abstract void Attack();
	protected abstract void Dead();
	
	public void GainHP(float amount)
	{
		hp = Mathf.Clamp (hp + amount, 0f, maxHp);
	}
	
	public void LoseHP(float amount)
	{
		hp = Mathf.Clamp (hp - amount, 0f, maxHp);
		
		if (hp == 0f)
			Dead ();
	}
}