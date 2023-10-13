using UnityEngine;
using System.Collections;

public class Npc_ForceWindState : NpcState 
{
	float ForceWindTime=0f;
	
	float speed=0f;
	float distance=0f;
	
	public Vector3 pushDir;
	public bool IsActive=false;
	
	public override void SetNPC (NpcBase _npc)
	{
		base.SetNPC (_npc);
	}
	
	public override void Enter()
	{
		ForceWindTime=0f;
		speed = 0f;
		distance=0f;
		IsActive=false;
	}
	
	public override void Execute()
	{
		if(!IsActive) return;
		
		ForceWindTime += Time.deltaTime;
		
		if(ForceWindTime > 2f)
		{
			if(Random.Range(0,100) < npc.AttackOnDamageChance)
			{
				npc.FSM.ChangeState( npc.AS );
			}
			else
			{
				npc.FSM.ChangeState( npc.IS );
			}
			return;
		}
		else
		{
			//push away by forcewind
			speed += 6.0f;
			speed = Mathf.Clamp(speed,0f,20f);
			distance += (pushDir * speed * Time.deltaTime).magnitude;
			if(distance <3f)
			{
				Owner.position += pushDir * speed * Time.deltaTime;
				RaycastHit hit;
				if(Physics.Raycast(Owner.position+Vector3.up*5f, Vector3.down,out hit,15f,1<<LayerMask.NameToLayer("Walkable")))
					
				{
					Owner.position = hit.point;
				}
				
			}
			npc.PlayIdleAnim();
		}
		
	}
	
	public override void Exit()
	{
	}	
}
