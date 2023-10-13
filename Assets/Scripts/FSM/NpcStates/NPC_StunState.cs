using UnityEngine;
using System.Collections;

public class NPC_StunState : NpcState 
{
	bool bTakeDamage = false;
	
	public override void Enter()
	{
		npc.PlayStunAnim();
		
	}
	
	public override void Execute()
	{
		if( npc.AnimationModel.animation.IsPlaying(npc.DamageAnim.name))
		{
			bTakeDamage = true;
		}
		else
		{
			if(bTakeDamage)
			{
				npc.PlayStunAnim();
				bTakeDamage = false;
			}
		}
	}
	
	public override void Exit()
	{
		
		
	}
}
