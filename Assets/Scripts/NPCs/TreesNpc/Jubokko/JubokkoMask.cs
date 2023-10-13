using UnityEngine;
using System.Collections;

public class JubokkoMask : InteractiveHandler 
{
	bool IsHurt=false;
	
	public AnimationClip IdleAnim;
	
	public AnimationClip ExetremeAnim;
	
	
	public override void Start ()
	{
		base.Start ();
		
		
		if(IdleAnim != null)
		{
           animation[IdleAnim.name].layer=-1;
           animation[IdleAnim.name].wrapMode = WrapMode.Loop;
		}
		
		if(ExetremeAnim != null)
		{
			animation[ExetremeAnim.name].layer = -1;
			animation[ExetremeAnim.name].wrapMode = WrapMode.Once;
		}
		
//		animation["JubkMask02_Damage_1"].layer=-1;
//		animation["JubkMask02_Damage_1"].wrapMode = WrapMode.Once;
//		animation["JubkMask02_PreDeath"].layer=-1;
//		animation["JubkMask02_PreDeath"].wrapMode = WrapMode.Once;
	}
	
//	public override void Active (int damage)
//	{
//		
//		Health -= damage;
//		if(Health<=0)
//		{
//			transform.parent = null;
//			gameObject.SetActiveRecursively(false);
//			Player.Instance.AttackTarget=null;
//		}
//		else
//		{
//			if(Health<10)
//				animation.CrossFade("JubkMask02_PreDeath");
//			else
//				animation.CrossFade("JubkMask02_Damage_1");
//			IsHurt=true;
//		}
//		
//	}
	
	public override void Update()
	{
//		if(IsHurt)
//		{
//			if(animation.isPlaying==false)
//			{
//				animation.CrossFade("JubkMask02_Idle");
//				IsHurt=false;
//			}
//		}
		
		base.Update();
	}
	
	public override void PlayImpactAnim()
	{
		if(Health < 10)
		{
		   if(ExetremeAnim != null)
				transform.animation.Play(ExetremeAnim.name);
		}
		else
		{
		   if( ImpactAnim != null)
			   transform.animation.Play(ImpactAnim.name);
		}
	}
	
	
}
