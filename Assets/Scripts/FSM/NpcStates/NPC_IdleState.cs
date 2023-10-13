using UnityEngine;
using System.Collections;

public class NPC_IdleState : NpcState {
	
	bool bIgnoreAttack=false;
	
	float IdleTime=0f;
	float PlayIdleSoundGap=0f;
	
	public override void Enter()
	{
		if(Player.Instance!=null && Player.Instance.AllEnemys.IndexOf(Owner)!= -1)
		{
			Player.Instance.AllEnemys.Remove(Owner);
		}

		npc.PlayIdleAnim();
		
		bIgnoreAttack=false;
		IdleTime=0f;
		PlayIdleSoundGap = Random.Range(0f,3.0f);
		
		if( NpcBase.AttackSound != null)
				NpcBase.AttackSound = null;
		//Debug.Log("entry Idle state.");
	}
	
	public override void Execute()
	{
		//player idle sound
		IdleTime += Time.deltaTime;
		if(IdleTime > PlayIdleSoundGap && Owner.GetComponent<NpcSoundEffect>()!=null)
		{
			if(NpcBase.IdleSound == null)
			   NpcBase.IdleSound = npc.PlaySound(Owner.GetComponent<NpcSoundEffect>().IdleSoundPrefab,NpcBase.IdleSound);
			IdleTime = 0f;
			PlayIdleSoundGap = Random.Range(3.0f,6.0f);
		}

		//search Player
		/*
		if(!Player.Instance.IsDead && !bIgnoreAttack)
		{
			Vector3 Offset  = (Player.Instance.transform.position - Owner.position);
			float angle =  Vector3.Angle( Vector3.Normalize(Offset), Owner.forward);
			if(Offset.magnitude < npc.VisionRadius && angle < npc.VisionAngle*0.5f )
			{
				int chance = Random.Range(0,100);
				if(chance < npc.AttackOnEnemySightChance && (Player.Instance.transform.position - npc.SpawnPoint).magnitude < npc.MovableRadius) 
				{
					if(npc.bMovable && !npc.IsTooCrowd())
					{
						npc.FSM.ChangeState(npc.AlertState);
					}
				}
				else
				{
					bIgnoreAttack=true;
				}
			}
		}
		*/
		
		if(!npc.AnimationModel.animation.IsPlaying(npc.IdleAnim.name) && !npc.AnimationModel.animation.IsPlaying(npc.DamageAnim.name))
			npc.PlayIdleAnim();
		
		
	}
	
	public override void Exit()
	{
		//print("exit idle state");
	}	
	
}
