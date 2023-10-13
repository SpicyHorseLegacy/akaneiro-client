using UnityEngine;
using System.Collections;

public class NpcGlobalState : NpcState {

	float OldWanderSpeed;
	float OldAttackMoveSpeed;
	float CaltropDamageTime=0f;
	float CaltropDamageDuration=1f;
	
	public bool IsCaltropDamage=false;
	public Transform CaltropSlowEffect;
	
	public override void Enter()
	{
		OldWanderSpeed = npc.SpeedWanderState;
		
	}
	
	public override void Execute()
	{
		if(npc.IsShadowBladeDamageEffect)
		{
			npc.SpeedWanderState = OldWanderSpeed * 0.5f;
			
			
			npc.ShadowBladeDamageEffectTime -= Time.deltaTime;
			if(npc.ShadowBladeDamageEffectTime<0f)
			{
				npc.ShadowBladeDamageEffectTime=0f;
				npc.IsShadowBladeDamageEffect=false;
				npc.SpeedWanderState = OldWanderSpeed;
				
			}
		}
	
		if(IsCaltropDamage)
		{
			npc.SpeedWanderState = OldWanderSpeed * 0.5f;
			
			
			if(CaltropSlowEffect)
			{
				CaltropSlowEffect.position = Owner.position;
				CaltropSlowEffect.rotation = Owner.rotation;
			}
			
			CaltropDamageTime += Time.deltaTime;
			if(CaltropDamageTime> CaltropDamageDuration)
			{
				CaltropDamageTime=0f;
				IsCaltropDamage = false;
				npc.SpeedWanderState = OldWanderSpeed;
				
				
				if(CaltropSlowEffect)
				{
					CaltropSlowEffect.gameObject.SetActiveRecursively(false);
				}
			}
		}
	}
	
	public override void Exit()
	{
	}	
}
