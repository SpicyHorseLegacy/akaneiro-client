using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_WanderState : NpcState {
	
	float AngleDiff=0f;
	
	Vector3 direction = Vector3.zero;
	
	public float StateSpeed = 0;
	
	public List<Vector3> LastMoveTarget = new List<Vector3>();
	
	
	public override void SetNPC (NpcBase _npc)
	{
		base.SetNPC (_npc);
		
		StateSpeed = npc.SpeedWanderState;
		
		npc.bFindPath=false;
		npc.bReachTargetPoint=false;
	}
	
	public NPC_WanderState(NpcBase o)
	{
		Owner=o.transform;
		npc = o;
	}
	
	public override void Enter()
	{
	    npc.bFindPath=false;
	    npc.IsPathfinding=false;
		
		npc.bReachTargetPoint = false;
		
		npc.curPathPointIndex=0;
		
		npc.bWanderReverse = false;
		
		npc.PlayWanderAnim();
	}
	
	public override void Execute()
	{
		//MoveToTarget();
		npc.WanderOnPath(LastMoveTarget,StateSpeed);
	}
	
	
	public void AdjustPosition()
	{
		RaycastHit hit;
				
        int layer = 1<<LayerMask.NameToLayer("Walkable");
		
		
		for(int i = 0; i < LastMoveTarget.Count;i++)
		{
			Vector3 temp = LastMoveTarget[i];
		
		    temp.y = 100;
		
            if(Physics.Raycast(temp,Vector3.down,out hit,100f,layer))
			{
		       temp.y = hit.point.y;
			}
			
			LastMoveTarget[i] = temp;
		}
	}
	
	
	public override void Exit()
	{
		npc.bFindPath=false;
		//npc.bReachTargetPoint = true;
		
		RestoreSpeed();
		
	}
	
	
	public void RestoreSpeed()
	{
		StateSpeed = npc.SpeedWanderState;
		
		if(npc.WalkAnim != null)
		{
			npc.AnimationModel.animation[npc.WalkAnim.name].speed = 1f;
			
		}
	}
	
	public void ChangSpeed(float ftoss)
	{
		StateSpeed *= ftoss;
		
		if(npc.WalkAnim != null)
		{
			npc.AnimationModel.animation[npc.WalkAnim.name].speed = ftoss;
			
		}
		
	}
	
}
