using UnityEngine;
using System.Collections;

public class ReviveState : PlayerState {

	public override void Enter()
	{
		print("revive!!!");
		player.PlayReviveAnim();
   	}
	
	public override void Execute()
	{
        player.FSM.ChangeState(player.abilityManager.GetAbilityByID((uint)AbilityIDs.Revive_ID));
	}
	
	public override void Exit()
	{
		player.IsDead = false;
	}
}
