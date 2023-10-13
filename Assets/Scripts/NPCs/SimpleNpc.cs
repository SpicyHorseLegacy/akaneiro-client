using UnityEngine;
using System.Collections;

public class SimpleNpc : MonoBehaviour {
	
	public int Health = 20;
	public int AttackDamage=3;
	
	public float AttackRadius = 1f;
	public float AvoidanceRadius=1f; 	
	
	public Transform AttackSound;	
	
	[HideInInspector]
	public Transform Attack_Sound;
	
	// Use this for initialization
	public virtual void Start () 
	{
		AttackRadius = 0.5f;
		AvoidanceRadius=0.5f; 	
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
	
	}
	
	public virtual void TakeDamage(int damage)
	{
		Health -= damage;
		
		if(Health<0)
		{
			PreDead();
			Destroy(gameObject);
		}
	}
	
	//do somethine before dead
	public virtual void PreDead()
	{
	}
	
	public virtual void AttackPlayer ()	
	{
		//play attack sound
		if(AttackSound && !Attack_Sound)
		{
			Attack_Sound = Object.Instantiate(AttackSound,transform.position,transform.rotation) as Transform;
		}

		if(Attack_Sound)
			SoundCue.Play(Attack_Sound.gameObject);
	}
}
