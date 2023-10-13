using UnityEngine;
using System.Collections;

public class Flee : BaseBuff {

	Transform vfx;
	
	NpcBase npc;
	
	public override void Enter()
	{
		base.Enter();
		
		if(Owner)
		{
			if(VFXPrefab && !vfx)
			{
				vfx = Instantiate(VFXPrefab, Owner.position + Vector3.up * 1.2f, VFXPrefab.rotation) as Transform;
				vfx.parent = Owner;
			}
			
			npc = Owner.GetComponent<NpcBase>();
			
		    if(npc)
			{
				npc.FSM.ChangeState(npc.FleeState);
			}
		}
	}
	
	public override void Execute(){
		base.Execute();
	}
	
	public override void Exit()
	{
		if(vfx)
			vfx.GetComponent<DestructAfterTime>().DestructNow();
		
		if(npc)
		{
		  if(npc.FSM.IsInState(npc.DS) == false)
		  { 
		    npc.PlayIdleAnim();
		    if(npc.FSM.IsInState(npc.FleeState))
			   npc.FleeState.ExitState();
		  }
		}
		
		base.Exit();
	}
}
