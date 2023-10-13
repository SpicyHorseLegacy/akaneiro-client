using UnityEngine;
using System.Collections;

public class NPC_AlertState : NpcState {
	
	public override void Enter()
	{
		if(npc.IsTooCrowd())
		   npc.FSM.ChangeState(npc.IS);
		 else
			npc.PlayOnAlertAnim();
	}
	
	public override void Execute()
	{
		//turn to player
		Vector3 ToTargetDir = (Player.Instance.transform.position - Owner.position).normalized;
		Owner.rotation = Quaternion.Slerp (Owner.rotation, Quaternion.LookRotation(ToTargetDir), 5f * Time.deltaTime);
		
		if(!npc.AnimationModel.animation.isPlaying || npc.AlertAnim==null)
		{
			if(npc.IsTooCrowd())
				npc.FSM.ChangeState(npc.IS);
			else
				npc.FSM.ChangeState(npc.CS);
		}
	}
	
	public override void Exit()
	{
	}
}
