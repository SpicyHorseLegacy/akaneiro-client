using UnityEngine;
using System.Collections;
using System;

public class NPC_KnockBackState : NpcState 
{
	public Vector3 KnockBackSpeed = Vector3.zero;
	
	float lastTime = 0;
	
	bool bStop = false;
	
	public Vector3 LastMovePosition = Vector3.zero;
	
	float  NpcStepLength = 0f;
	
	//Vector3 oldForward = Vector3.zero;
	
	public NPC_KnockBackState(NpcBase o)
	{
		Owner=o.transform;
		npc = o;
	}
	
	public override void Enter()
	{
		lastTime = Time.realtimeSinceStartup;
		
		NpcStepLength = KnockBackSpeed.magnitude;
		
		LastMovePosition = Owner.position;
		
		bStop = false;
		
		npc.PlayKnockBackAnim();
	}
	
	public override void Execute()
	{
		MoveToTarget();
	}
	
	public override void Exit()
	{
		bStop = false;	
	}
	
	public void ExitState()
	{
		bStop = true;

        if (npc.FSM.GetPreviousState() == npc.CS || npc.FSM.GetPreviousState() == npc.WS)
            npc.FSM.ChangeState(npc.FSM.GetPreviousState());
	}
    
	public  void MoveToTarget()
	{
		if(bStop)
			return;
		
        Vector3 temp = Owner.position;
		
		float realTime = Time.realtimeSinceStartup;
		
		float deltatime = realTime - lastTime;
		
		lastTime = realTime;
		
		/*
		LastMovePosition.y = temp.y;
		
		temp = Vector3.Lerp(temp,LastMovePosition,NpcStepLength * deltatime);
		
		
		
		RaycastHit hit;
		
		if(Physics.Raycast(temp + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
		{
			temp.y = hit.point.y;
		}
		
		Vector3 OffsetPoint = temp - LastMovePosition;
		
		OffsetPoint.y = 0;
		*/
		/*
		if(OffsetPoint.magnitude < 0.5f)
		{
			npc.PlayIdleAnim();
		}
		else
		{
			npc.PlayKnockBackAnim();
		}
		*/
		if( npc.KnockBackAnim != null && !npc.AnimationModel.animation.IsPlaying(npc.KnockBackAnim.name))
			npc.PlayIdleAnim();
			
		
		//Owner.position = temp;
		
		
		temp += KnockBackSpeed * deltatime;
		
		//lastTime = realTime;
			
        RaycastHit hit;
	
		AstarClasses.Int3 TargetPos = AstarPath.ToLocal(temp);
		
		if(TargetPos.x == -1 && TargetPos.y == -1 && TargetPos.z == -1)
		{
			bStop = true;
			
			npc.PlayIdleAnim();
			
			//Debug.Log( "Monster " + npc.ObjID + " cannot KnockBack NULL");
			
		    return;
		}
		
	    AstarClasses.Node  TargetNode = AstarPath.GetNode(TargetPos);
		
		if(!TargetNode.walkable )
		{
			bStop = true;
			
			npc.PlayIdleAnim();
			
			//Debug.Log( "Monster " + npc.ObjID + " cannot KnockBack Unwalkable");
			
		    return;
		}
		else
		{
		   if(Physics.Raycast(temp + Vector3.up*5f,Vector3.down,out hit,20f,1 << LayerMask.NameToLayer("Walkable")))
		   {
			  temp.y = hit.point.y;
		   }
		   Owner.position = temp;
		}
		
		
		
	}
}
