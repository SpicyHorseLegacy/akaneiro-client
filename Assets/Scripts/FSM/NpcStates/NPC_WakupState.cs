using UnityEngine;
using System.Collections;

public class NPC_WakupState : NpcState {
	
	public override void Enter()
	{
		if(npc.FSM.IsInState(npc.DS))
			return;
		npc.PlayWakeupAnim();
	}
	
	public override void Execute()
	{
		//turn to player
		//Vector3 ToTargetDir = (Player.Instance.transform.position - Owner.position).normalized;
		//Owner.rotation = Quaternion.Slerp (Owner.rotation, Quaternion.LookRotation(ToTargetDir), 5f * Time.deltaTime);		
		
		//if(!Owner.animation.isPlaying || npc.WakeupAnim ==null)
		//{
			//npc.FSM.ChangeState(npc.CS);
		//}
	    if(!npc.AnimationModel.animation.isPlaying)
		{
			npc.PlayIdleAnim();
		}
	}
	
	public override void Exit()
	{
		//int yu = 0;
	}
}
