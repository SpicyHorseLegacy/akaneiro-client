using UnityEngine;
using System.Collections;

public class Npc_FleeState : NpcState 
{
	public Vector3 KnockBackSpeed = Vector3.zero;
	
	float lastTime = 0;
	
	bool bStop = false;
	
	public Vector3 LastMovePosition = Vector3.zero;
	
	float  NpcStepLength = 0f;
	
	//Vector3 oldForward = Vector3.zero;
	
	public Npc_FleeState(NpcBase o)
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
		
		npc.PlayRunAnim();
		
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
		
		npc.PlayIdleAnim();
		
		if(npc.FSM.GetPreviousState() == npc.CS || npc.FSM.GetPreviousState() == npc.WS)
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
		
		Owner.forward = KnockBackSpeed;
		
		temp += KnockBackSpeed * deltatime;
				
        RaycastHit hit;
		
		AstarClasses.Int3 TargetPos = AstarPath.ToLocal(temp);
		
		if(TargetPos.x == -1 && TargetPos.y == -1 && TargetPos.z == -1)
		{
			bStop = true;
			
			npc.PlayIdleAnim();
			
			//Debug.Log( "Monster " + npc.ObjID + " cannot Flee NULL");
			
		    return;
		}
		
	    AstarClasses.Node  TargetNode = AstarPath.GetNode(TargetPos);
		
		if(!TargetNode.walkable )
		{
			bStop = true;
			
			npc.PlayIdleAnim();
			
			//Debug.Log( "Monster " + npc.ObjID + " cannot Flee Unwalkable");
			
		    return;
		}
		else
		{
			if(Physics.Raycast(temp + Vector3.up*5f,Vector3.down,out hit,20f,1 << LayerMask.NameToLayer("Walkable")))
		    {
			   temp.y = hit.point.y;
		    }
			
			Owner.position = temp;
			npc.PlayRunAnim();
		}
	}
}


