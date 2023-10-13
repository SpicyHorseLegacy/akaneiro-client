using UnityEngine;
using System.Collections;

public class Fruit_DeathState : NPC_DeathState 
{
	NpcBase npc;
	Transform DeathSound;
	
	public Fruit_DeathState(Transform o)
	{
		Owner=o;
		npc = Owner.GetComponent<NpcBase>();
	}
	
	public override void Enter()
	{
		//Debug.Log("entry death state.");
		if(Player.Instance.AllEnemys.IndexOf(Owner)!= -1)
		{
			Player.Instance.AllEnemys.Remove(Owner);
			if(npc.AroundIndex!=-1)
			{
				Player.Instance.AllAroundPos[npc.AroundIndex]=false;
				npc.AroundIndex=-1;
			}			
		}
		npc.PlayDeathAnim();
		
		//play death sound
		npc.CommonKilledSound = npc.PlaySound(Owner.GetComponent<NpcSoundEffect>().CommonKilledSoundPrefab,npc.CommonKilledSound);
		npc.DeathSound = npc.PlaySound(Owner.GetComponent<NpcSoundEffect>().DeathSoundPrefab,npc.DeathSound);
		
		//particle
		if(npc.DeathEffect && npc.DeathEffectInst==null)
		{
			npc.DeathEffectInst = Object.Instantiate(npc.DeathEffect,Owner.position,Owner.rotation) as Transform;
		}
		
//		Player.Instance.PlayKaramCollectEffect();
	}
	
	public override void Execute()
	{
		if(npc.IsDeathAnimFinished())
		{
			npc.DeathAnimationFinished();
			//npc.Owner.GetComponent<Jubokko>().FruitCount--;
		}
	}
	
	public override void Exit()
	{
	}

}
