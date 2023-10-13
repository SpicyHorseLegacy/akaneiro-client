using UnityEngine;
using System.Collections;

public class DeathState : PlayerState {

	public override void Enter()
	{
        base.Enter();
        print("Player dead!");
		player.PlayDeathAnim();
		player.IsDead =true;
        player.CanActiveAbility = false;
        player.GetComponent<PlayerMovement>().StopMove(false);
        player.SoundHandler.PlayDeathSound();
//        _UI_CS_Revival.Instance.AwakeRevival();
		
#if NGUI
		GUIManager.Instance.ChangeUIScreenState("Revive_Screen");
#else
		RevivePanel.Instance.AwakeRevival();
#endif
		
		GameCamera.EnterDeathState();
	}
	
	public override void Execute()
	{
		base.Execute();
	}
	
	public override void Exit()
	{
		base.Execute();
        player.GetComponent<PlayerMovement>().bStopMove = false;
        player.CanActiveAbility = true;
		GameCamera.BackToPlayerCamera();
		print("Exit Death");
	}
}
